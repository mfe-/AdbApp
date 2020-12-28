using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AdbApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IAdbService adbService;
        private readonly IDialogService dialogService;

        public MainPageViewModel(INavigationService navigationService, IAdbService adbService, IDialogService dialogService)
            : base(navigationService)
        {
            Title = "Log Page";
            Params = "logcat";
            this.adbService = adbService;
            this.dialogService = dialogService;
        }

        private ObservableCollection<string> _Logs;
        public ObservableCollection<string> Logs
        {
            get { return _Logs; }
            set { SetProperty(ref _Logs, value, nameof(Logs)); }
        }


        private String _Params;
        public String Params
        {
            get { return _Params; }
            set { SetProperty(ref _Params, value, nameof(Params)); }
        }

        private ICommand _GetAdbLogCommand;

        public ICommand GetAdbLogCommand => _GetAdbLogCommand ?? (_GetAdbLogCommand = new DelegateCommand(OnGetAdbLogCommand));

        protected void OnGetAdbLogCommand()
        {
            try
            {
                adbService.GetAdbLog("");
            }
            catch(Exception e)
            {
                dialogService.ShowDialog(e.ToString());
            }
        }
    }
}
