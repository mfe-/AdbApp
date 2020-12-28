using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdbApp.Droid
{
    public class AdbService : IAdbService
    {
        public AdbService()
        {
        }
        public Task<IList<string>> GetAdbLog(string param)
        {
            List<string> logs = new List<string>();

            var processBuilder = new ProcessBuilder();
            processBuilder.Command(param);

            var redirect = processBuilder.RedirectOutput();

            var process = processBuilder.Start();

            BufferedReader bufferedReader = new BufferedReader(
                     new InputStreamReader(process.InputStream));

            string s = bufferedReader.ReadLine();

            logs.Add(s);
            return Task.FromResult<IList<string>>(logs);
        }
    }
}