namespace MauiCoreLibrary.Helpers;

public class TimeStamp
{
    public static string TimeStampDashed => $"{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}";
}
