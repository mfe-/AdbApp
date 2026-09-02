using System.Windows.Input;

namespace AdbApp.Maui.ViewModels;

public class AboutPageViewModel : ViewModelBase
{
    public AboutPageViewModel()
    {
        TapCommand = new Command<string>(async url =>
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                await Launcher.Default.OpenAsync(url);
            }
        });
    }

    public ICommand TapCommand { get; }
}
