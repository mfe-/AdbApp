using AdbApp.Maui.Android;
using AdbApp.Maui.ViewModels;
using AdbApp.Maui.Views;

namespace AdbApp.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
            });

        builder.Services.AddSingleton<IAdbService, AdbService>();
        builder.Services.AddSingleton<IToastService, ToastService>();
        builder.Services.AddSingleton<IClipBoardService, ClipBoardService>();

        builder.Services.AddSingleton<AdbPageViewModel>();
        builder.Services.AddSingleton<AboutPageViewModel>();

        builder.Services.AddSingleton<AdbPage>();
        builder.Services.AddSingleton<AboutPage>();
        builder.Services.AddSingleton<PredefinedCommandPage>();
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}
