using M3u8Downloader_H.Abstractions.M3u8;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Abstractions.Plugins.Download;
using M3u8Downloader_H.Common.Extensions;
using M3u8Downloader_H.Common.M3u8;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.Demo.Plugins.Services
{
    internal class PluginDownload : IDownloadService
    {
        private IM3uKeyInfo m3UKeyInfo = default!;
        private readonly IDownloadService downloadService;
        private readonly IDownloadContext downloadContext;

        //此方法 在当前类中不使用
        public Func<Stream, CancellationToken, Stream> HandleDataFunc { get; set; } = default!;

        //此方法 在当前类中不使用
        public Func<string, Stream, CancellationToken, Task> WriteToFileFunc { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public PluginDownload(IDownloadService downloadService, IDownloadContext downloadContext)
        {
            this.downloadService = downloadService;
            this.downloadContext = downloadContext;
            //给原始的HandleDataFunc赋值一个新的自己实现的方法
            downloadService.HandleDataFunc = HandleData;

            //同理如果需要修改WriteToFile你只需要按照下面的方法修改即可
            //downloadService.WriteToFileFunc = MyWriteToFile;
        }

        public Task<bool> DownloadM3uMediaInfo(IM3uMediaInfo m3UMediaInfo, IEnumerable<KeyValuePair<string, string>>? headers, string mediaPath, CancellationToken cancellationToken = default)
        {
            //此方法由自己调用 外部不会调用这个方法 
            //接口申明这个方法的目的 是为了让你可以使用接口里实现的DownloadM3uMediaInfo
            throw new NotImplementedException();
        }


        public async ValueTask BeforeDownload(IM3uFileInfo m3UFileInfo, CancellationToken cancellationToken)
        {
            if (m3UKeyInfo is not null)
                return;

            if (m3UFileInfo.Key.Uri != null && m3UFileInfo.Key.BKey == null)
            {
                try
                {
                    byte[] data = m3UFileInfo.Key.Uri.IsFile
                        ? await File.ReadAllBytesAsync(m3UFileInfo.Key.Uri.OriginalString, cancellationToken)
                        : await downloadContext.HttpClient.GetByteArrayAsync(m3UFileInfo.Key.Uri, downloadContext.DownloadParam.Headers, cancellationToken);

                    downloadContext.Log?.Info("获取转为base64的密钥 : {0}", Convert.ToBase64String(data));
                    m3UKeyInfo = M3uKeyInfoHelper.GetKeyInfoInstance(m3UFileInfo.Key.Method, data, m3UFileInfo.Key.IV);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    throw new HttpRequestException("密钥获取失败");
                }
                catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new HttpRequestException("获取密钥失败，没有找到任何数据", e.InnerException, e.StatusCode);
                }
            }
        }


        public async ValueTask Initialization(CancellationToken cancellationToken = default)
        {
            downloadContext.Log.Info("进入Initialization");
            //调用原始方法
            await downloadService.Initialization(cancellationToken);
            downloadContext.Log.Info("将要退出Initialization");
        }

        public async Task StartDownload(IM3uFileInfo m3UFileInfo, CancellationToken cancellationToken = default)
        {
            downloadContext.Log.Info("进入StartDownload");
            await BeforeDownload(m3UFileInfo, cancellationToken);
            //调用原始StartDownload
            await downloadService.StartDownload(m3UFileInfo, cancellationToken);
            downloadContext.Log.Info("退出StartDownload");
        }

        public Stream HandleData(Stream stream, CancellationToken cancellationToken)
        {
            downloadContext.Log.Info("进入HandleData方法");
            //这里没有调用原始方法是因为 原始的方法只处理默认的数据流 无法处理非标准加密
            //比如魔改aes 甚至有自己实现的别的什么加密方式等 你都可以重写这个方法实现自己的
            return stream.AesDecrypt(m3UKeyInfo.BKey, m3UKeyInfo.IV);
        }
    }
}
