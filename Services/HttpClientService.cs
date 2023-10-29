namespace MauiCoreLibrary.Services;

public class HttpClientService : IHttpClientService
{
    private readonly IAlertService _alert;
    private readonly IFileLogService _log;

    public HttpClientService(IAlertService alert, IFileLogService log)
    {
        _alert = alert;
        _log = log;
    }

    public async Task<Dictionary<string, object>> PostJsonAsync(string uri, string json)
    {
        Dictionary<string, object> postResponse = new();

        try
        {
            using HttpClient client = new();
            StringContent content = new(json, Encoding.UTF8, "application/json");
            _log?.AppendLine($"New send POST request to {uri} request with content:\n{json}");
            using HttpResponseMessage httpResponse = await client.PostAsync(uri, content);
            _log?.AppendLine($"POST requested response: {httpResponse.StatusCode}");

            if (httpResponse.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                _log?.AppendLine($"POST response content:\n{jsonResponse}");
                postResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);
            }
            else
                throw new Exception($"Request unsuccessful!\n\nRequest message:\n{httpResponse.RequestMessage}\n\nStatus code:\n{httpResponse.StatusCode}\n\nReason:\n{httpResponse.ReasonPhrase}");

        }
        catch (Exception ex)
        {
            _log?.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"Request not successful.\nError message: {ex.Message}", "Ok");
        }

        return postResponse;
    }
}
