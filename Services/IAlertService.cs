namespace MauiCoreLibrary.Services;

public interface IAlertService
{
    Task DisplayAlertAsync(string title, string message, string confirmation);
    Task<bool> DisplayAlertAsync(string title, string message, string confirmation, string cancelation);
}
