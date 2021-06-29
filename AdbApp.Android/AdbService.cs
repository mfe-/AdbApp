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
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdbApp.Droid
{
    public class AdbService : IAdbService
    {
        private const int bufferSize = 128;

        private CancellationTokenSource cancellationTokenSource;

        public AdbService()
        {
        }

        public async Task<IList<string>> GetAdbOutputAsync(string param, Action<string> callback = null)
        {
            if (string.IsNullOrEmpty(param)) throw new ArgumentException(nameof(param));
            using (cancellationTokenSource = new CancellationTokenSource())
            {
                string[] commandParameter = param.Split(" ").Where(a => a != string.Empty).ToArray();

                List<string> logs = new List<string>();
                string workingDir = SysProp.GetProp("user.dir");
                using (var processBuilder = new ProcessBuilder())
                {
                    if (!System.String.IsNullOrWhiteSpace(workingDir))
                    {
                        processBuilder.Directory(new Java.IO.File(workingDir));
                    }

                    processBuilder.RedirectErrorStream(true);
                    processBuilder.Command(commandParameter);
                    using (var process = processBuilder.Start())
                    {
                        using (BufferedReader bufferedInputReader = new BufferedReader(
                                 new InputStreamReader(process.InputStream)))
                        {
                            await ReadStreamAsync(bufferedInputReader, logs, callback, cancellationTokenSource.Token);
                        }
                        process.Destroy();
                        process.Dispose();
                    }
                }
                cancellationTokenSource = null;
                return logs;
            }
        }
        private async Task ReadStreamAsync(Reader bufferedReader, IList<string> logs, Action<string> callback = null, CancellationToken? cancellationToken = null)
        {
            string s;
            char[] buffer = new char[bufferSize];
            int readAmountChars = 0;
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(buffer.Length * 2);
            do
            {
                //wait one second for ReadAsync to return otherwise cancel
                Task<int> readAsyncTask = bufferedReader.ReadAsync(buffer, 0, buffer.Length);
                var completedTask = await Task.WhenAny(readAsyncTask, Task.Delay(TimeSpan.FromSeconds(1),
                    cancellationTokenSource.Token));
                if (completedTask != readAsyncTask)
                {
                    readAmountChars = 0;
                    cancellationTokenSource.Cancel();
                }
                else
                {
                    readAmountChars = readAsyncTask.Result;
                }
                //read from buffer array
                for (int i = 0; i < readAmountChars; i++)
                {
                    char c = buffer[i];
                    if (c != '\n')
                    {
                        stringBuilder.Append(c);
                    }
                    else
                    {
                        s = stringBuilder.ToString();
                        callback?.Invoke(s);
                        logs.Add(s);
                        stringBuilder.Clear();
                    }
                }
                buffer = new char[bufferSize];
                //check if a cancellation was requested
                if (cancellationToken != null && cancellationToken.Value.IsCancellationRequested)
                    break;
            }
            while (readAmountChars > 0);
        }

        public void StopAdbOutputAsync()
        {
            try
            {
                cancellationTokenSource?.Cancel();
            }
            catch (ObjectDisposedException)
            {

            }

        }
    }
}