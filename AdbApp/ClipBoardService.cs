using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AdbApp
{
    public class ClipBoardService : IClipBoardService
    {
        public Task SetTextAsync(string text)
            => Clipboard.SetTextAsync(text);
    }
}
