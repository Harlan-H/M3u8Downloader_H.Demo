using Avalonia.Controls;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Abstractions.Plugins.Window;
using M3u8Downloader_H.Demo.ViewModel;
using M3u8Downloader_H.Demo.Views;


namespace M3u8Downloader_H.Demo
{
    //当需要gui的时候实现此接口
    public class Gui : IWindowPlugin
    {
        private IWindowContext windowContext = default!;

        public Type ViewType => typeof(MainWindowView);

        public void InitializeWindow(IWindowContext windowContext)
        {
            this.windowContext = windowContext;
        }

        public object CreateMainView()
        {
            return new MainWindowViewModel(windowContext);
        }
    }
}
