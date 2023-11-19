namespace MauiCoreLibrary.Models;

public interface IAppSettingsModel
{
    string AppName { get; }
    string AppDataDirectory { get; }
    string AppDocumentsDirectory { get; }
}