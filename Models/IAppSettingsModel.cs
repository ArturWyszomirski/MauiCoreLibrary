namespace MauiCoreLibrary.Models;

public interface IAppSettingsModel
{
    string ApplicationDataDirectory { get; }
    string AppName { get; }
}