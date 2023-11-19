using Microsoft.VisualBasic.FileIO;

namespace MauiCoreLibrary.Models;

public class AppSettingsModel : ModelBase, IAppSettingsModel
{
    public AppSettingsModel()
    {
#if WINDOWS
        AppDataDirectory = SpecialDirectories.CurrentUserApplicationData;
        AppDocumentsDirectory = SpecialDirectories.MyDocuments;
#elif ANDROID
        AppDataDirectory = (string)Android.App.Application.Context.GetExternalFilesDir((string)Android.OS.Environment.DataDirectory);
        AppDocumentsDirectory = (string)Android.App.Application.Context.GetExternalFilesDir((string)Android.OS.Environment.DirectoryDocuments);
#endif
    }
    
    public string AppName { get; } = Assembly.GetCallingAssembly().GetName().Name;
    public string AppDataDirectory { get; }
    public string AppDocumentsDirectory { get; }
}
