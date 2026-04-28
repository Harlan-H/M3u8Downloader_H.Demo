using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Demo;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace M3u8Downloader_H.Gui.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Main dllMain;

        [ObservableProperty]
        public partial UserControl  MainView { get; set; }


        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<IWindowContext>();
            dllMain = new Main();
            var windowPlugin =  dllMain.CreateWindoPlugin();
            windowPlugin!.InitializeWindow(context);
            MainView = windowPlugin.CreateMainView()!;
        }


    }
}
