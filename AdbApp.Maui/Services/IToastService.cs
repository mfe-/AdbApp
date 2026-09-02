namespace AdbApp.Maui.Services;

public interface IToastService
{
    Task ShowToastAsync(string message);
}
