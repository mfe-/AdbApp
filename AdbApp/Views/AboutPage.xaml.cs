using AdbApp.Maui.ViewModels;

namespace AdbApp.Maui.Views;

public partial class AboutPage : ContentPage
{
    public AboutPage(AboutPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
