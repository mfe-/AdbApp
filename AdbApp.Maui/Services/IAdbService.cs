namespace AdbApp.Maui.Services;

public interface IAdbService
{
    Task<IList<string>> GetAdbOutputAsync(string param, Action<string>? callback = null);

    void StopAdbOutputAsync();
}
