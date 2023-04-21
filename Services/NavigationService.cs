namespace MauiCoreLibrary.Services;

/// <summary>
/// To get full functionality of this service all viewmodels should inherit ViewModelBase.
/// </summary>
public class NavigationService : INavigationService
{
    #region Fields
    private readonly IServiceProvider _serviceProvider;
    private object[] _parameters;
    private ShellNavigationState _previousState;
    #endregion

    #region Constructors
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Shell.Current.Navigated += async (s, e) => await CallOnNavigatedToAsync();
        Shell.Current.Navigating += async (s, e) => await CallOnNavigatingFromAsync(); // navigating isn't fired when navigating using tabbar - MAUI framework bug
    }
    #endregion

    #region Properties
    private static INavigation Navigation => Application.Current.MainPage.Navigation;
    private static Page CurrentPage => Shell.Current.CurrentPage;
    public ShellItem CurrentItem => Shell.Current.CurrentItem; // Current item is the first item in shell hierarchy. Items nested inside the first item are available at CurrentItem.CurrentItem and so on...
    public ShellNavigationState CurrentState => Shell.Current.CurrentState;
    #endregion

    #region Public methods
        #region URI-based navigation
    public async Task GoToAsync(string route, params object[] parameters)
    {
        _parameters = parameters;
        await Shell.Current.GoToAsync(route);
    }

    public async Task GoToAsync(string route, bool animate = true, params object[] parameters)
    {
        _parameters = parameters;
        await Shell.Current.GoToAsync(route, animate);
    }

    public async Task GoBackAsync(params object[] parameters)
    {
        _parameters = parameters;
        await Shell.Current.GoToAsync(_previousState);
    }

    public async Task GoBackAsync(bool animate, params object[] parameters)
    {
        _parameters = parameters;
        await Shell.Current.GoToAsync(_previousState, animate);
    }
        #endregion

        #region Modeless navigation
    public async Task PushAsync(Page page, params object[] parameters)
    {
        _parameters = parameters;
        await Navigation.PushAsync(page);
    }

    public async Task PushAsync(Page page, bool animate = true, params object[] parameters)
    {
        _parameters = parameters;
        await Navigation.PushAsync(page, animate);
    }

    public async Task PushAsync<T>(params object[] parameters) where T : Page
    {
        _parameters = parameters;
        Page page = _serviceProvider.GetService<T>();
        await Navigation.PushAsync(page);
    }

    public async Task PushAsync<T>(bool animate = true, params object[] parameters) where T : Page
    {
        _parameters = parameters;
        Page page = _serviceProvider.GetService<T>();
        await Navigation.PushAsync(page, animate);
    }

    public async Task PopAsync(params object[] parameters)
    {
        _parameters = parameters;
        await Navigation.PopAsync();
    }

    public async Task PopAsync(bool animated = true, params object[] parameters)
    {
        _parameters = parameters;
        await Navigation.PopAsync(animated);
    }
        #endregion
    #endregion

    #region Private methods
    private async Task CallOnNavigatedToAsync()
    {
        if (CurrentPage.BindingContext is ViewModelBase viewModel)
            await viewModel.OnNavigatedToAsync(_parameters);
    }

    private async Task CallOnNavigatingFromAsync()
    {
        _previousState = CurrentState;
        if (CurrentPage.BindingContext is ViewModelBase viewModel)
            await viewModel.OnNavigatingFromAsync(_parameters);
    }
    #endregion
}
