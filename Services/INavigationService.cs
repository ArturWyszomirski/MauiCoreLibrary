namespace MauiCoreLibrary.Services
{
    public interface INavigationService
    {
        public ShellItem CurrentItem { get; }
        public ShellNavigationState CurrentState { get; }
        public ShellNavigationState PreviousState { get; }

        /* URI-based navigation */
        Task GoToAsync(string route, bool animate = true, params object[] parameters);
        Task GoToAsync(string route, params object[] parameters);
        Task GoBackAsync(params object[] parameters);
        Task GoBackAsync(bool animate, params object[] parameters);

        /* Modeless navigation */
        Task PopAsync(bool animated = true, params object[] parameters);
        Task PopAsync(params object[] parameters);
        Task PushAsync(Page page, bool animate = true, params object[] parameters);
        Task PushAsync(Page page, params object[] parameters);
        Task PushAsync<T>(bool animate = true, params object[] parameters) where T : Page;
        Task PushAsync<T>(params object[] parameters) where T : Page;
    }
}