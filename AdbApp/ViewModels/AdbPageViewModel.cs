using AdbApp.Maui;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace AdbApp.Maui.ViewModels;

public class AdbPageViewModel : ViewModelBase
{
    private readonly IAdbService adbService;
    private readonly IClipBoardService clipBoardService;
    private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

    private string command;
    private string filter = string.Empty;
    private bool processingAdbOutput;
    private int outputCount;

    public AdbPageViewModel(IAdbService adbService, IClipBoardService clipBoardService)
    {
        Title = "adb shell";
        command = Preferences.Default.Get(nameof(Command), "logcat - D *:W");

        this.adbService = adbService;
        this.clipBoardService = clipBoardService;

        Output = new ObservableCollection<string>();
        Output.CollectionChanged += OnOutputCollectionChanged;

        GetAdbCommand = new Command<string>(async p => await OnGetAdbCommandAsync(p));
        CancelCommand = new Command(OnCancelCommand);
        ClearCommand = new Command(OnClearCommand);
        CopyCommand = new Command<string>(async text => await OnCopyAsync(text));
    }

    public ObservableCollection<string> Output { get; }

    public IReadOnlyList<string> FilterOutput
    {
        get
        {
            if (string.IsNullOrEmpty(Filter))
            {
                return Output.ToList();
            }

            return Output.Where(line => line.Contains(Filter, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    public string Command
    {
        get => command;
        set
        {
            if (SetProperty(ref command, value))
            {
                Preferences.Default.Set(nameof(Command), command);
            }
        }
    }

    public string Filter
    {
        get => filter;
        set
        {
            if (SetProperty(ref filter, value))
            {
                OnPropertyChanged(nameof(FilterOutput));
            }
        }
    }

    public bool ProcessingAdbOutput
    {
        get => processingAdbOutput;
        set => SetProperty(ref processingAdbOutput, value);
    }

    public int OutputCount
    {
        get => outputCount;
        set => SetProperty(ref outputCount, value);
    }

    public ICommand GetAdbCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand CopyCommand { get; }

    private async Task OnCopyAsync(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        await clipBoardService.SetTextAsync(text);
    }

    private async Task OnGetAdbCommandAsync(string? param)
    {
        if (string.IsNullOrWhiteSpace(param))
        {
            return;
        }

        try
        {
            await semaphoreSlim.WaitAsync();
            ProcessingAdbOutput = true;
            adbService.StopAdbOutputAsync();
            AddOutputLine(param);
            _ = await adbService.GetAdbOutputAsync(param, AddOutputLine);
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Output.Clear();
                Output.Add(ex.ToString());
                OnPropertyChanged(nameof(FilterOutput));
            });
        }
        finally
        {
            ProcessingAdbOutput = false;
            semaphoreSlim.Release();
        }
    }

    private void OnClearCommand()
    {
        Output.Clear();
        OnPropertyChanged(nameof(FilterOutput));
    }

    private void OnCancelCommand()
    {
        adbService.StopAdbOutputAsync();
        ProcessingAdbOutput = false;
    }

    private void AddOutputLine(string line)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Output.Add(line);
            OnPropertyChanged(nameof(FilterOutput));
        });
    }

    private void OnOutputCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OutputCount = Output.Count;
    }
}
