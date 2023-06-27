namespace MauiCoreLibrary.Services;

public interface IHttpClientService
{
    Task<List<Dictionary<string, object>>> PostJsonAsync(string uri, string json);
}