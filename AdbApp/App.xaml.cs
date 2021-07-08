using AdbApp.ViewModels;
using AdbApp.Views;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AdbApp
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("/MasterDetailPage/NavigationPage/AdbPage");

        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);

            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Views.MasterDetailPage, MasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<AdbPage, AdbPageViewModel>();
            containerRegistry.RegisterForNavigation<PredefinedCommandPage, PredefinedCommandPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();

        }

        //protected override void OnInitialized()
        //{

        //    InitializeComponent();

        //   
        //}

        //protected override void RegisterTypes(IContainerRegistry containerRegistry)
        //{
        //    containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

        //    containerRegistry.RegisterForNavigation<NavigationPage>();
        //    containerRegistry.RegisterForNavigation<Views.MasterDetailPage, MasterDetailPageViewModel>();
        //    containerRegistry.RegisterForNavigation<AdbPage, AdbPageViewModel>();
        //    containerRegistry.RegisterForNavigation<PredefinedCommandPage, PredefinedCommandPageViewModel>();
        //    containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
        //}
    }
}
