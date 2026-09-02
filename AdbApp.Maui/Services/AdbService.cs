using Java.IO;
using Java.Lang;
using Reader = Java.IO.Reader;

namespace AdbApp.Maui.Services;

public class AdbService : IAdbService
{
    private const int BufferSize = 128;
    private CancellationTokenSource? cancellationTokenSource;

    public async Task<IList<string>> GetAdbOutputAsync(string param, Action<string>? callback = null)
    {
        if (string.IsNullOrWhiteSpace(param))
        {
            throw new ArgumentException(nameof(param));
        }

        using (cancellationTokenSource = new CancellationTokenSource())
        {
            string[] commandParameter = param.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            List<string> logs = new();
            string workingDir = SysProp.GetProp("user.dir");
            using var processBuilder = new ProcessBuilder();

            if (!string.IsNullOrWhiteSpace(workingDir))
            {
                processBuilder.Directory(new Java.IO.File(workingDir));
            }

            processBuilder.RedirectErrorStream(true);
            processBuilder.Command(commandParameter);

            using var process = processBuilder.Start();
            if (process is not null)
            {
                using BufferedReader bufferedInputReader = new(new InputStreamReader(process.InputStream));
                await ReadStreamAsync(bufferedInputReader, logs, cancellationTokenSource.Token, callback);
                process.Destroy();
            }

            cancellationTokenSource = null;
            return logs;
        }
    }

    private async Task ReadStreamAsync(Reader bufferedReader, IList<string> logs, CancellationToken cancellationToken, Action<string>? callback = null)
    {
        char[] buffer = new char[BufferSize];
        int readAmountChars;
        System.Text.StringBuilder stringBuilder = new(buffer.Length * 2);

        do
        {
            Task<int> readAsyncTask = bufferedReader.ReadAsync(buffer, 0, buffer.Length);
            Task completedTask = await Task.WhenAny(readAsyncTask, Task.Delay(TimeSpan.FromSeconds(1), cancellationToken));
            if (completedTask != readAsyncTask)
            {
                readAmountChars = 0;
                cancellationTokenSource?.Cancel();
            }
            else
            {
                readAmountChars = readAsyncTask.Result;
            }

            for (int i = 0; i < readAmountChars; i++)
            {
                char c = buffer[i];
                if (c != '\n')
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    string line = stringBuilder.ToString();
                    callback?.Invoke(line);
                    logs.Add(line);
                    stringBuilder.Clear();
                }
            }

            buffer = new char[BufferSize];
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
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
