namespace MauiCoreLibrary.Services;

public class FileReportService : FileLogService, IFileReportService
{
    private readonly IAlertService _alert;

    readonly object _appendLock = new();

    public FileReportService(IAlertService alert, IAppSettingsModel appSettings) : base(alert, appSettings)
    {
        _alert = alert;
    }

    protected override string DirectoryName => "Reports";
    protected override string FileSuffix => "Report";

    public void Append(params string[] texts)
    {
        lock (_appendLock)
        {
            try
            {
                using StreamWriter streamWriter = File.AppendText(Path.Join(FilePath, FileName));
                foreach (string text in texts)
                    streamWriter.Write(text);
            }
            catch (Exception ex) 
            {
                _alert.DisplayAlertAsync("Error", $"{ex.Message}", "Ok");
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
                using StreamWriter streamWriter = File.AppendText(Path.Join(FilePath, FileName));

                foreach (string line in lines)
                    streamWriter.WriteLine(line);

                streamWriter.Close();
            }
            catch (Exception ex)
            {
                _alert?.DisplayAlertAsync("Error", $"${ex.Message}", "Ok");
                Debug.WriteLine(ex);
            }
        }
    }
}