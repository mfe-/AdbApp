using System.Threading.Tasks;

namespace AdbApp
{
    public interface IToastService
    {
        Task ShowToastAsync(string message);
    }
}
