namespace MauiCoreLibrary.Services;

/// <summary>
/// To use this service invoke CreateFile() before appending text.
/// </summary>
public class FileLogService(IAlertService alert = null, IAppSettingsModel appSettings = null) : IFileLogService
{
    private readonly IAlertService _alert = alert;
    private readonly IAppSettingsModel _appSettings = appSettings;

    readonly object _readLock = new();
    readonly object _appendLock = new();

    public string FilePath { get; protected set; }
    public string FileName { get; protected set; }
    protected virtual string DirectoryName => "Logs";
    protected virtual string FileSuffix => "Log";
    protected string FileType { get; set; } = "txt";

    /// <summary>
    /// If <paramref name="filePath"/> or <paramref name="fileName"/> not provided creates a file with default values default values (filePath: MyDocuments/AppName).
    /// </summary>
    /// <param name="filePath"></param>
    /// or Path.Combine(_appSettings?.AppDataDirectory, DirectoryName) in Android.</param>
    /// <exception cref="Exception"></exception>
    public void CreateFile(string filePath = null, string fileName = null)
    {
        if (string.IsNullOrEmpty(filePath))
            if (!string.IsNullOrEmpty(_appSettings?.AppName))
                if (!string.IsNullOrEmpty(_appSettings?.AppDataDirectory))
                {
#if WINDOWS
                    FilePath = Path.Combine(_appSettings?.AppDataDirectory, _appSettings?.AppName, DirectoryName);
#elif ANDROID
                    FilePath = Path.Combine(_appSettings?.AppDataDirectory, DirectoryName);
#endif
                }
                else throw new ArgumentNullException(_appSettings.AppDataDirectory);
            else throw new ArgumentNullException(_appSettings.AppName);
        else
            FilePath = filePath;

        if (string.IsNullOrEmpty(fileName))
            FileName = $"{TimeStamp.TimeStampDashed}_{FileSuffix}.{FileType}";
        else
            FileName = fileName;

        try
        {
            Directory.CreateDirectory(FilePath);
            using StreamWriter streamWriter = File.CreateText(Path.Join(FilePath, FileName));
        }
        catch (Exception ex)
        {
            _alert?.DisplayAlertAsync("Error", $"${ex.Message}", "Ok");
            Debug.WriteLine(ex);
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

                streamWriter.WriteLine();
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                _alert?.DisplayAlertAsync("Error", $"${ex.Message}", "Ok");
                Debug.WriteLine(ex);
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
            catch (Exception ex)
            {
                _alert?.DisplayAlertAsync("Error", $"${ex.Message}", "Ok");
                throw;
            }
        }
    }
}