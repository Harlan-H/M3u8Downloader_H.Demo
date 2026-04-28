using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Common.DownloadPrams;
using M3u8Downloader_H.Demo.FrameWork;
using M3u8Downloader_H.Demo.Interfaces;
using M3u8Downloader_H.Demo.Utils;

namespace M3u8Downloader_H.Demo.ViewModel
{
    public partial class MainWindowViewModel(IWindowContext windowContext)  : IPluginViewModelBase
    {
        //private readonly Http<IGithubApi> http = new(windowContext.ApiFactory, "https://api.github.com");

        [ObservableProperty]
        public partial string ShowText { get; set; } = "show text";

        [ObservableProperty]
        public partial string Url { get; set; }

        private bool CanTestClick() => !string.IsNullOrEmpty(ShowText);

        [RelayCommand(CanExecute = nameof(CanTestClick))]
        private void TestClick()
        {
            ShowText = " private void TestClick";
            windowContext.SnackbarMaranger.Notify("plugin TestClick");

//             var api = http.GetApi();
//             var resp = api.GetUser("harlan");
            try
            {
                var downloadParam = new M3u8DownloadParams(new Uri(Url), null, string.Empty, "mp4", null);
                windowContext.AppCommandService.DownloadByUrl(downloadParam, "demo");
            }
            catch (Exception ex)
            {
                windowContext.SnackbarMaranger.Notify(ex.Message);
            }

        }
    }
}
