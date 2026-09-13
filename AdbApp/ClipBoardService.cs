namespace AdbApp.Maui;

public class ClipBoardService : IClipBoardService
{
    private readonly IToastService toastService;

    public ClipBoardService(IToastService toastService)
    {
        this.toastService = toastService;
    }

    public async Task SetTextAsync(string text)
    {
        await Clipboard.Default.SetTextAsync(text);
        await toastService.ShowToastAsync("Copied!");
    }
}
