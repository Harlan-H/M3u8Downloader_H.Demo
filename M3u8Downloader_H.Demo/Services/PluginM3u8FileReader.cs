using M3u8Downloader_H.Abstractions.M3u8;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Abstractions.Plugins.Download;

namespace M3u8Downloader_H.Demo.Services
{
    internal class PluginM3u8FileReader(IM3uFileReader m3UFileReader, IDownloadContext downloadContext) : IM3uFileReader
    {
        public void InitAttributeReade(IAttributeReaderCollection readers)
        {
            downloadContext.Log.Info("进入InitAttributeReade 外部会实际调用这个方法");
            downloadContext.Log.Info($" current {readers.Count} size");
            //调用默认处理方法 以保证程序正常执行
            //如果是一些魔改m3u8 比如他是以某种json形式存在 并不是标准的m3u8文件结构
            //且在GetM3u8FileInfo中不会调用m3UFileReader.GetM3u8FileInfo
            //那就可以不执行此方法
            m3UFileReader.InitAttributeReade(readers);
            //这里的AttributeReade方法主要是为了可以实现一些非标准的m3u8标签
            //例如 #EXT-X-Custom-Key 然后继承IAttributeReader
            //readers.Add("#EXT-X-Custom-Key",MyCustomKeyReader)
        }

        public IM3uFileInfo GetM3u8FileInfo(Stream stream)
        {
            downloadContext.Log.Info("进入GetM3u8FileInfo");
            var ret = m3UFileReader.GetM3u8FileInfo(stream);
            downloadContext.Log.Info($"获得到的ts流数量是:{ret.MediaFiles.Count}");
            return ret;
        }

    }
}
