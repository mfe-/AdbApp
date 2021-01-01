using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace AdbApp.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        public MasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigateCommand = new DelegateCommand<string>(OnNavigateCommand);
        }

        public ICommand NavigateCommand { get; }

        protected async void OnNavigateCommand(string navigateToPage)
        {
            try
            {
                var result = await NavigationService.NavigateAsync($"NavigationPage/{navigateToPage}");
            }
            catch (Exception e)
            {

            }
        }
    }
}
