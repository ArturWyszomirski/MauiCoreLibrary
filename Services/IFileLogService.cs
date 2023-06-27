namespace MauiCoreLibrary.Services;

public interface IFileLogService
{
    string FilePath { get; }
    string FileName { get; }

    void AppendLine(params string[] lines);
    void CreateFile(string filePath = null, string fileName = null);
    string ReadFile();
}