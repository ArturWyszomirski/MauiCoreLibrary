namespace MauiCoreLibrary.Models;

public class AppSettingsModel : IAppSettingsModel
{
    public string ProductName { get; protected set; }

    public string ApplicationDataDirectory { get; protected set; }
}
