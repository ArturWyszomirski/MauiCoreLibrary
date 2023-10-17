namespace MauiCoreLibrary.Models;

public interface IAppSettingsModel
{
    string AppDataDirectory { get; }
    string AppName { get; }
}