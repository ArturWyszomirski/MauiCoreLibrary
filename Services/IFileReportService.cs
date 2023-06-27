namespace MauiCoreLibrary.Services;

public interface IFileReportService : IFileLogService
{
    new string FileName { get; }

    void Append(params string[] texts);
}