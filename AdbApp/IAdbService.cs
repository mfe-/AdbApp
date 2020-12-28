using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdbApp
{
    public interface IAdbService
    {
        Task<IList<String>> GetAdbLog(string param);
    }
}
