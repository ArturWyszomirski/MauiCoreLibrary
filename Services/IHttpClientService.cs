namespace MauiCoreLibrary.Services;

public interface IHttpClientService
{
    Task<Dictionary<string, object>> PostJsonAsync(string uri, string json);
}