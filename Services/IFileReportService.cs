namespace MauiCoreLibrary.Services;

public interface IFileReportService : IFileLogService
{
    new string FileName { get; set; }

    void Append(params string[] texts);
}