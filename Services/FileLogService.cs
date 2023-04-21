using System.Diagnostics;

namespace MauiCoreLibrary.Services;

public class FileLogService : IFileLogService
{
    readonly object _readLock = new();
    readonly object _appendLock = new();

    public FileLogService(IAppSettingsModel appSettings)
    {
        FilePath = Path.Combine(appSettings.ApplicationDataDirectory, "Logs");
        FileName = $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}_Log.txt";
    }

    public string FilePath { get; protected set; }

    public string FileName { get; protected set; }

    public void CreateFile(string filePath, string fileName)
    {
        FilePath = filePath;
        FileName = fileName;

        try
        {
            Directory.CreateDirectory(filePath);
            using StreamWriter streamWriter = File.CreateText(Path.Join(filePath, fileName));
        }
        catch
        {
            //dodać exceptiony
            throw;
        }
    }

    public virtual void AppendLine(params string[] lines)
    {
        lock (_appendLock)
        {
            try
            {
                using StreamWriter streamWriter = File.AppendText(Path.Join(FilePath, FileName));
                streamWriter.WriteLine($"[{DateTime.Now:O}]");

                foreach (string line in lines)
                    streamWriter.WriteLine(line);

                streamWriter.Close();
            }
            catch
            {
                // TODO: add exceptions
                throw;
            }
#if DEBUG
            Debug.WriteLine($"[{DateTime.Now:O}]");
            foreach (string line in lines) { Debug.WriteLine(line); }
#endif
        }
    }

    public string ReadFile()
    {
        lock (_readLock)
        {
            try
            {
                using StreamReader streamReader = File.OpenText(Path.Join(FilePath, FileName));
                return streamReader.ReadToEnd();
            }
            catch
            {
                //dodać exceptiony
                throw;
            }
        }
    }
}