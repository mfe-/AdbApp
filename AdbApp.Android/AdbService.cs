using Java.IO;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace AdbApp.Droid
{
    public class AdbService : IAdbService
    {
        private const int bufferSize = 128;

        private CancellationTokenSource? cancellationTokenSource;
        public async Task<IList<string>> GetAdbOutputAsync(string param, Action<string>? callback = null)
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
                        if (process != null)
                        {
                            using (BufferedReader bufferedInputReader = new BufferedReader(
                                     new InputStreamReader(process.InputStream)))
                            {
                                await ReadStreamAsync(bufferedInputReader, logs, cancellationTokenSource.Token, callback);
                            }
                            process.Destroy();
                            process.Dispose();
                        }
                    }
                }
                cancellationTokenSource = null;
                return logs;
            }
        }
        private async Task ReadStreamAsync(Reader bufferedReader, IList<string> logs, CancellationToken cancellationToken, Action<string>? callback = null)
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
                    cancellationToken));
                if (completedTask != readAsyncTask)
                {
                    readAmountChars = 0;
                    cancellationTokenSource?.Cancel();
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
                if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                    break;
            }
            while (readAmountChars > 0);
        }

        void IAdbService.StopAdbOutputAsync()
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