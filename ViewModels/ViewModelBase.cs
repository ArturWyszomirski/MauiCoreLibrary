namespace MauiCoreLibrary.ViewModels;

public abstract class ViewModelBase : ObservableRecipient
{
    public virtual async Task OnNavigatedToAsync(params object[] parameters) => await Task.CompletedTask;
    public virtual async Task OnNavigatingFromAsync(params object[] parameters) => await Task.CompletedTask;
}