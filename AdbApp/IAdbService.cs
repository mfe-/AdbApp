using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AdbApp
{
    public interface IAdbService
    {
        Task<IList<String>> GetAdbOutputAsync(string param, Action<string>? callback = null);

        void StopAdbOutputAsync();
    }
}
