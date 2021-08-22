using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AdbApp
{
    public class ClipBoardService : IClipBoardService
    {
        private readonly IToastService toastService;

        public ClipBoardService(IToastService toastService)
        {
            this.toastService = toastService;
        }

        public Task SetTextAsync(string text)
        {
            Clipboard.SetTextAsync(text);
            return toastService.ShowToastAsync("Copied!");
        }
    
    }
}
