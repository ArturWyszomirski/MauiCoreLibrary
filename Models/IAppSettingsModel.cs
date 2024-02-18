namespace MauiCoreLibrary.Models;

public interface IAppSettingsModel
{
    string AppName { get; }
    string AppDataDirectory { get; }
    string AppDocumentsDirectory { get; }

    public JsonSerializerOptions JsonSerializerOptions { get; }
}