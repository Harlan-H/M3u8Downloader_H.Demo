using M3u8Downloader_H.Abstractions.Plugins;
using M3u8Downloader_H.Abstractions.Plugins.Download;
using M3u8Downloader_H.Abstractions.Plugins.Window;
using M3u8Downloader_H.Attributes.Attributes;

namespace M3u8Downloader_H.Demo
{
    [Plugin("demo测试","这是一个demo测试dll","Harlan","5.0.0",Key = "demo",HasDownload = true,HasUi = true)]
    public class Main : IPluginEntry
    {
        public bool CanHandle(Uri url)
            => true;

        public IDownloadPlugin? CreateDownloadPlugin()
            => new Download();

        public IWindowPlugin? CreateWindoPlugin()
            => new Gui();
    }
}
