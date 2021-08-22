using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;

namespace AdbApp.ViewModels
{
    public class AdbPageViewModel : ViewModelBase
    {
        private readonly IAdbService adbService;
        private readonly IClipBoardService clipBoardService;

        public AdbPageViewModel(INavigationService navigationService, IAdbService adbService, IClipBoardService clipBoardService)
            : base(navigationService)
        {
            Title = "adb shell";
            this._Command = Preferences.Get(nameof(Command), "logcat - D *:W");
            this.adbService = adbService;
            this.clipBoardService = clipBoardService;
            this._Output = new ObservableCollection<string>();
            this.GetAdbCommand = new DelegateCommand<string>(OnGetAdbCommandAsync);
            this.CancelCommand = new DelegateCommand(OnCancelCommand);
            this.ClearCommand = new DelegateCommand(OnClearCommand);
            this.FilterCommand = new DelegateCommand<string>(OnFilter);
            this.CopyCommand = new DelegateCommand<string>(OnCopyAsync);
        }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        private void OnCopyAsync(string text)
            => clipBoardService.SetTextAsync(text);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        public ICommand CopyCommand { get; }
        private ObservableCollection<string> _Output;
        public ObservableCollection<string> Output
        {
            get { return _Output; }
            set { SetProperty(ref _Output, value, nameof(Output)); }
        }

        public ObservableCollection<string> FilterOutput
        {
            get
            {
                if (String.IsNullOrEmpty(Filter))
                {
                    return _Output;
                }
                else
                {
                    return new ObservableCollection<string>(Output.Where(a => a.Contains(Filter) || String.IsNullOrEmpty(Filter)));
                }
            }
        }

        private string _Command;
        public string Command
        {
            get { return _Command; }
            set
            {
                SetProperty(ref _Command, value, nameof(Command));
                Preferences.Set(nameof(Command), _Command);
            }
        }

        private string _Filter = String.Empty;
        public string Filter
        {
            get { return _Filter; }
            set { SetProperty(ref _Filter, value, nameof(Filter)); }
        }

        public ICommand FilterCommand { get; }
        private void OnFilter(string filter)
        {
            Filter = filter;
            RaisePropertyChanged(nameof(FilterOutput));
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
                Output.Add(param);
                _ = await adbService.GetAdbOutputAsync(param, s => Output.Add(s));
            }
            catch (ArgumentException)
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
            => Output.Clear();

        public ICommand CancelCommand { get; }

        protected void OnCancelCommand()
        {
            adbService.StopAdbOutputAsync();
            ProcessingAdbOutput = false;
        }
    }
}
