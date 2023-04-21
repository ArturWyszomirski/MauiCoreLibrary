namespace MauiCoreLibrary.Services;

public class FileReportService : FileLogService, IFileReportService
{
    readonly object _appendLock = new();

    public FileReportService(IAppSettingsModel appSettings) : base(appSettings)
    {
        FilePath = Path.Combine(appSettings.ApplicationDataDirectory, "Reports");
    }

    public new string FileName { get; set; }

    public void Append(params string[] texts)
    {
        lock (_appendLock)
        {
            try
            {
                using StreamWriter streamWriter = File.AppendText(Path.Join(base.FilePath, FileName));
                foreach (string text in texts)
                    streamWriter.Write(text);
            }
            catch
            {
                //dodać exceptiony
                throw;
            }
        }
    }

    public override void AppendLine(params string[] lines)
    {
        lock (_appendLock)
        {
            try
            {
                using StreamWriter streamWriter = File.AppendText(Path.Join(base.FilePath, FileName));
                foreach (string line in lines)
                    streamWriter.WriteLine(line);
            }
            catch
            {
                //dodać exceptiony
                throw;
            }
        }
    }
}