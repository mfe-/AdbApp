using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;

namespace AdbApp.ViewModels
{
    public class AboutPageViewModel : BindableBase
    {
        public ICommand TapCommand => new DelegateCommand<string>(async (url) => await Launcher.OpenAsync(url));
    }
}
