namespace AdbApp.Maui;

public interface IToastService
{
    Task ShowToastAsync(string message);
}
