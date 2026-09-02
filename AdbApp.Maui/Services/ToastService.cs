using Android.Widget;
using Microsoft.Maui.ApplicationModel;

namespace AdbApp.Maui.Services;

public class ToastService : IToastService
{
    public Task ShowToastAsync(string message)
    {
        var context = Platform.CurrentActivity ?? Platform.AppContext;
        Toast.MakeText(context, message, ToastLength.Short)?.Show();
        return Task.CompletedTask;
    }
}
