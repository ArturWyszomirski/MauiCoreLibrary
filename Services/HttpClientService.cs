namespace MauiCoreLibrary.Services;

public class HttpClientService
{
    public static async Task<List<Dictionary<string, object>>> PostJsonAsync(string uri, string json)
    {
        using HttpClient client = new();
        StringContent content = new(json, Encoding.UTF8, "application/json");
        using HttpResponseMessage response = await client.PostAsync(uri, content);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponse = await response.Content.ReadAsStringAsync(); 
            return JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonResponse);
        }
        else
            throw new Exception($"Request unsuccessful!\n\nRequest message:\n{response.RequestMessage}\n\nStatus code:\n{response.StatusCode}\n\nReason:\n{response.ReasonPhrase}");
    }
}
