namespace MauiCoreLibrary.Services;

public class AlertService : IAlertService
{
    public async Task DisplayAlertAsync(string title, string message, string confirmation)
    {
        if (Shell.Current is not null)
            await Shell.Current.DisplayAlert(title, message, confirmation);
        else
        {
            string errorMessage = $"{nameof(Shell.Current)} is null."; // try-catch won't handle exception from UI thread
            Debug.WriteLine(errorMessage);
        }
    }

    public async Task<bool> DisplayAlertAsync(string title, string message, string confirmation, string cancelation)
    {
        if (Shell.Current is not null)
            return await Shell.Current.DisplayAlert(title, message, confirmation, cancelation);
        else
        {
            string errorMessage = $"{nameof(Shell.Current)} is null."; // try-catch won't handle exception from UI thread
            Debug.WriteLine(errorMessage);
            throw new Exception(errorMessage);
        }
    }
}
