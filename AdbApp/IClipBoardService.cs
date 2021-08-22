using System.Threading.Tasks;

namespace AdbApp
{
    public interface IClipBoardService
    {
        Task SetTextAsync(string text);
    }
}
