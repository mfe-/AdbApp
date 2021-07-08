using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdbApp.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
            _title = String.Empty;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
    
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
  
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public void Destroy()
        {

        }
    }
}
