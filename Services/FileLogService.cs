using Microsoft.VisualBasic.FileIO;

namespace MauiCoreLibrary.Services;

public class FileLogService : IFileLogService
{
    private readonly IAlertService _alert;
    private readonly IAppSettingsModel _appSettings;

    readonly object _readLock = new();
    readonly object _appendLock = new();

    public FileLogService(IAlertService alert, IAppSettingsModel appSettings)
    {
        _alert = alert;
        _appSettings = appSettings;
    }

    public string FilePath { get; protected set; }
    public string FileName { get; protected set; }
    protected virtual string DirectoryName => "Logs";
    protected virtual string FileSuffix => "Log";
    protected string FileType { get; set; } = "txt";

    /// <summary>
    /// If <paramref name="filePath"/> or <paramref name="fileName"/> not provided creates a file with default values default values (filePath: MyDocuments/AppName).
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <exception cref="Exception"></exception>
    public void CreateFile(string filePath = null, string fileName = null)
    {
        if (string.IsNullOrEmpty(filePath))
            if (!string.IsNullOrEmpty(_appSettings?.AppName))
                FilePath = Path.Combine(SpecialDirectories.MyDocuments, _appSettings?.AppName, DirectoryName);
            else throw new Exception($"{nameof(_appSettings.AppName)} is null or empty.");
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