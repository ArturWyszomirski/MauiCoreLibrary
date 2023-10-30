namespace MauiCoreLibrary.Services;

public class HttpClientService : IHttpClientService
{
    #region Fields
    private readonly IAlertService _alert;
    private readonly IFileLogService _log;
    #endregion

    #region Constructor
    public HttpClientService(IAlertService alert, IFileLogService log)
    {
        _alert = alert;
        _log = log;
    }
    #endregion

    #region Public methods
    public async Task<Dictionary<string, object>> PostJsonAsync(string uri, string json)
    {
        try
        {
            using HttpClient client = new();
            StringContent content = new(json, Encoding.UTF8, "application/json");

            _log?.AppendLine($"New send POST request to {uri} request with content:\n{json}");
            using HttpResponseMessage httpResponse = await client.PostAsync(uri, content);

            _log?.AppendLine($"POST requested response: {httpResponse.StatusCode}");
            return await GetResponseAsDictionary(httpResponse);
        }
        catch (Exception ex)
        {
            _log?.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"Request not successful.\nError message: {ex.Message}", "Ok");
            return null;
        }
    }

    public async Task<Dictionary<string, object>> PostFileAsync(string uri, string filePath, string mediaTypeHeader, string name = null, string fileName = null, string apiKey = null)
    {
        try
        {
            using HttpClient client = new();
            MultipartFormDataContent multipartFormData = CreateMultiPartFormDataContent(filePath, mediaTypeHeader, name, fileName);

            if (!string.IsNullOrEmpty(apiKey))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            _log?.AppendLine($"New send POST request to {uri} request with content: {filePath}");
            using HttpResponseMessage httpResponse = await client.PostAsync(uri, multipartFormData);

            _log?.AppendLine($"POST requested response: {httpResponse.StatusCode}");
            return await GetResponseAsDictionary(httpResponse);
        }
        catch (Exception ex)
        {
            _log?.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"Request not successful.\nError message: {ex.Message}", "Ok");
            return null;
        }
    }
    #endregion

    #region Private methods
    private async Task<Dictionary<string, object>> GetResponseAsDictionary(HttpResponseMessage httpResponse)
    {
        Dictionary<string, object> postResponse = new();
        if (httpResponse.IsSuccessStatusCode)
        {
            string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            _log?.AppendLine($"POST response content:\n{jsonResponse}");

            try
            {
                postResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);
            }
            catch (Exception ex)
            {
                _log?.AppendLine(ex.ToString());
                await _alert?.DisplayAlertAsync("Error", $"Error during decoding response.\nError message: {ex.Message}", "Ok");
            }
        }
        else
            _log?.AppendLine($"Request unsuccessful!",
                             $"Request message:\n{httpResponse.RequestMessage}",
                             $"Status code:\n{httpResponse.StatusCode}",
                             $"Reason:\n{httpResponse.ReasonPhrase}");
        
        return postResponse;
    }

    private static MultipartFormDataContent CreateMultiPartFormDataContent(string filePath, string mediaTypeHeader, string name, string fileName)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(filePath);
        ByteArrayContent content = new(bytes);
        content.Headers.ContentType = new(mediaTypeHeader);
        MultipartFormDataContent multipartFormData = new();

        if (fileName is not null && name is not null)
            multipartFormData.Add(content, name, fileName);
        else if (name is not null)
            multipartFormData.Add(content, name);
        else
            multipartFormData.Add(content);

        return multipartFormData;
    }
    #endregion
}
