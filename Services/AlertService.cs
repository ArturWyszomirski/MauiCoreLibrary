namespace MauiCoreLibrary.Services;

public class AlertService : IAlertService
{
    public async Task DisplayAlertAsync(string title, string message, string confirmation)
    {
        await Shell.Current.DisplayAlert(title, message, confirmation);
    }

    public async Task<bool> DisplayAlertAsync(string title, string message, string confirmation, string cancelation)
    {
        return await Shell.Current.DisplayAlert(title, message, confirmation, cancelation);
    }
}
