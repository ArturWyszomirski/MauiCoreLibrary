namespace MauiCoreLibrary.Services;

public interface IHttpClientService
{
    Task<Dictionary<string, object>> PostJsonAsync(string uri, string json);

    /// <summary>
    /// Parameter <paramref name="name"/> is a field name for API (not file name).
    /// Parameter <paramref name="fileName"/> is a file name given when file received on server side. If declared then <paramref name="name"/> must be also not null.
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="filePath"></param>
    /// <param name="mediaTypeHeader"></param>
    /// <param name="name">Field name for API (not file name).</param>
    /// <param name="fileName">File name given when file received on server side. If declared then <paramref name="name"/> must be also not null.</param>
    /// <param name="apiKey"></param>
    /// <returns></returns>

    Task<Dictionary<string, object>> PostFileAsync(string uri, string filePath, string mediaTypeHeader, string name = null, string fileName = null, string apiKey = null);
}