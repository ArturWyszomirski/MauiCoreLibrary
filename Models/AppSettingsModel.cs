namespace MauiCoreLibrary.Models;

public class AppSettingsModel : IAppSettingsModel
{
    public string AppName { get; protected set; } = Assembly.GetCallingAssembly().GetName().Name;

    public string ApplicationDataDirectory { get; protected set; }
}
