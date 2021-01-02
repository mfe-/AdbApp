using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace AdbApp.ViewModels
{
    public class AdbPageViewModel : ViewModelBase
    {
        private readonly IAdbService adbService;

        public AdbPageViewModel(INavigationService navigationService, IAdbService adbService)
            : base(navigationService)
        {
            Title = "adb shell";
            this._Params = "logcat -D *:W";
            this._Output = new ObservableCollection<string>();
            this.GetAdbCommand = new DelegateCommand<string>(OnGetAdbCommandAsync);
            this.CancelCommand = new DelegateCommand(OnCancelCommand);
            this.ClearCommand = new DelegateCommand(OnClearCommand);
            this.adbService = adbService;
        }

        private ObservableCollection<string> _Output;
        public ObservableCollection<string> Output
        {
            get { return _Output; }
            set { SetProperty(ref _Output, value, nameof(Output)); }
        }


        private string _Params;
        public string Params
        {
            get { return _Params; }
            set { SetProperty(ref _Params, value, nameof(Params)); }
        }

        private bool _ProcessingAdbOutput;
        public bool ProcessingAdbOutput
        {
            get { return _ProcessingAdbOutput; }
            set { SetProperty(ref _ProcessingAdbOutput, value, nameof(ProcessingAdbOutput)); }
        }

        public ICommand GetAdbCommand { get; }

        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        protected async void OnGetAdbCommandAsync(string param)
        {
            try
            {
                await semaphoreSlim.WaitAsync();
                ProcessingAdbOutput = true;
                adbService.StopAdbOutputAsync();
                _ = await adbService.GetAdbOutputAsync(param, s => Output.Add(s));
            }
            catch(ArgumentException)
            {
                //param is empty
            }
            catch (Exception e)
            {
                Output.Clear();
                Output.Add(e.ToString());
            }
            finally
            {
                ProcessingAdbOutput = false;
                semaphoreSlim.Release();
            }
        }

        public ICommand ClearCommand { get; }

        protected void OnClearCommand()
        {
            Output.Clear();
        }

        public ICommand CancelCommand { get; }

        protected void OnCancelCommand()
        {
            adbService.StopAdbOutputAsync();
            ProcessingAdbOutput = false;

        }
    }
}
