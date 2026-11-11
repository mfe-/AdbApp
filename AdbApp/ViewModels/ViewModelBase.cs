using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdbApp.Maui.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    private string title = string.Empty;

    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
        {
            return false;
        }

        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
