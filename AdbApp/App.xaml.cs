using AdbApp.ViewModels;
using AdbApp.Views;
using Prism;
using Prism.Ioc;
using Prism.Services;
using System;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace AdbApp
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {

            InitializeComponent();

            await NavigationService.NavigateAsync("/MasterDetailPage/NavigationPage/AdbPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterScoped<IClipBoardService, ClipBoardService>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Views.MasterDetailPage, MasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<AdbPage, AdbPageViewModel>();
            containerRegistry.RegisterSingleton<AdbPageViewModel>();
            containerRegistry.RegisterForNavigation<PredefinedCommandPage, PredefinedCommandPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
        }
    }
}
