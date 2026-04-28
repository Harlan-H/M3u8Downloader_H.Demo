using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Abstractions.Plugins.Download;
using M3u8Downloader_H.Demo.Plugins.Services;
using M3u8Downloader_H.Demo.Services;

namespace M3u8Downloader_H.Demo
{
    //当需要自己特定的服务的时候 实现此接口
    public class Download : IDownloadPlugin
    {
        //如果不需要直接返回null;
        public IDownloadService? CreateDownloadService(IDownloadService downloadService, IDownloadContext downloadContext)
            => new PluginDownload(downloadService, downloadContext);

        //如果不需要直接返回null;
        public IM3uFileReader? CreateM3uFileReader(IM3uFileReader m3UFileReader, IDownloadContext downloadContext) 
            => new PluginM3u8FileReader(m3UFileReader, downloadContext);
    }
}
