﻿namespace MauiCoreLibrary.Services;

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

    #region EventHandlers
    public EventHandler<SseUpdateReceivedEventArgs> SseUpdateReceived { get; set; }
    #endregion

    #region Properties
    public HttpResponseMessage ResponseMessage { get; private set; }
    #endregion

    #region Public methods
    public async Task<bool> GetAsync(Uri uri)
    {
        try
        {
            using HttpClient client = new();

            _log?.AppendLine($"New send GET request to {uri}.");
            ResponseMessage = await client.GetAsync(uri);
            await LogResponse();

            return true;
        }
        catch (Exception ex)
        {
            _log?.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"Request not successful.\nError message: {ex.Message}", "Ok");

            return false;
        }
    }

    public async Task<bool> PostJsonAsync(Uri uri, string json)
    {
        try
        {
            using HttpClient client = new();
            StringContent content = new(json, Encoding.UTF8, "application/json");

            _log?.AppendLine($"New send POST request to {uri} with content:\n{json}");
            ResponseMessage = await client.PostAsync(uri, content);
            await LogResponse();

            return true;
        }
        catch (Exception ex)
        {
            _log?.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"Request not successful.\nError message: {ex.Message}", "Ok");

            return false;
        }
    }

    public async Task<bool> PostFileAsync(Uri uri, string filePath, string mediaTypeHeader, string name = null, string fileName = null, string apiKey = null)
    {
        try
        {
            using HttpClient client = new();

            if (!string.IsNullOrEmpty(apiKey))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            _log?.AppendLine($"New send POST request to {uri} with content: {filePath}");
            using FileStream fileStream = File.OpenRead(filePath);
            using StreamContent content = new(fileStream);
            content.Headers.ContentType = new(mediaTypeHeader);
            ResponseMessage = await client.PostAsync(uri, content);
            await LogResponse();

            return true;
        }
        catch (Exception ex)
        {
            _log?.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"Request not successful.\nError message: {ex.Message}", "Ok");

            return false;
        }
    }

    public async Task<bool> PutJsonAsync(Uri uri, string json)
    {
        try
        {
            using HttpClient client = new();
            StringContent content = new(json, Encoding.UTF8, "application/json");

            _log?.AppendLine($"New send PUT request to {uri} with content:\n{json}");
            ResponseMessage = await client.PutAsync(uri, content);
            await LogResponse();

            return true;
        }
        catch (Exception ex)
        {
            _log?.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"Request not successful.\nError message: {ex.Message}", "Ok");

            return false;
        }
    }

    public async Task<bool> PutFileAsync(Uri uri, string filePath, string mediaTypeHeader, string name = null, string fileName = null, string apiKey = null)
    {
        try
        {
            using HttpClient client = new();

            if (!string.IsNullOrEmpty(apiKey))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            _log?.AppendLine($"New PUT request send to {uri} with content: {filePath}");
            using FileStream fileStream = File.OpenRead(filePath);
            using StreamContent content = new(fileStream);
            content.Headers.ContentType = new(mediaTypeHeader);
            ResponseMessage = await client.PutAsync(uri, content);
            await LogResponse();


            return true;
        }
        catch (Exception ex)
        {
            _log?.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"Request not successful.\nError message: {ex.Message}", "Ok");

            return false;
        }
    }

    public async Task<bool> SubscribeToSseAsync(Uri uri, CancellationToken stopToken, CancellationToken cancelToken)
    {
        try
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));
            _log?.AppendLine($"Subscribe to SSE request send to: {uri}");
            var sseStream = await client.GetStreamAsync(uri);

            await Task.Run(async () =>
            {
                using StreamReader reader = new(sseStream);
                while (!reader.EndOfStream)
                {
                    cancelToken.ThrowIfCancellationRequested();

                    if (stopToken.IsCancellationRequested)
                    {
                        _log?.AppendLine($"{uri} SSE has been stopped by client.");
                        break;
                    }

                    var line = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(line)) 
                        continue;

                    _log?.AppendLine($"SSE update message: {line}");
                    await MainThread.InvokeOnMainThreadAsync(() => {
                        SseUpdateReceived?.Invoke(this, new SseUpdateReceivedEventArgs { Message = line });
                    });
                }
            }, cancelToken);

            _log?.AppendLine($"End of {uri} SSE stream.");

            return true;
        }
        catch (Exception ex)
        {
            _log?.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"SSE request not successful.\nError message: {ex.Message}", "Ok");

            return false;
        }
    }

    public async Task<Dictionary<string, object>> GetResponseAsDictionary(HttpResponseMessage httpResponse)
    {
        Dictionary<string, object> postResponse = [];
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
                             $"Request message:", $"{httpResponse.RequestMessage}",
                             $"Status code:", $"{httpResponse.StatusCode}",
                             $"Reason:", $"{httpResponse.ReasonPhrase}");

        return postResponse;
    }
    #endregion

    #region Private methods
    private async Task LogResponse()
    {
        _log?.AppendLine($"POST requested response status code: {ResponseMessage.StatusCode}");
        string responseContent = await ResponseMessage.Content.ReadAsStringAsync();
        _log?.AppendLine($"POST requested response content: {responseContent}");
    }

    private static MultipartFormDataContent CreateMultiPartFormDataContent(string filePath, string mediaTypeHeader, string name, string fileName)
    {
        byte[] bytes = File.ReadAllBytes(filePath);
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

public class SseUpdateReceivedEventArgs
{
    public string Message { get; set; }
}