namespace MauiCoreLibrary.Services;

public class SerializeService : ISerializeService
{
    private readonly IAlertService _alert;
    private readonly IFileLogService _fileLog;

    public SerializeService(IAlertService alert, IFileLogService fileLog)
    {
        _alert = alert;
        _fileLog = fileLog;
    }

    public async Task<T> DeserializeObjectFromJsonFileAsync<T>(string directory, string fileName)
    {
        string filePath = Path.Combine(directory, fileName);
        try
        {
            using FileStream fileStream = File.OpenRead(filePath);
            DataContractJsonSerializer serializer = new(typeof(T));
            return (T)serializer.ReadObject(fileStream);
        }
        catch (Exception ex)
        {
            _fileLog.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"JSON serialization error: {ex.Message}", "Ok");
            return default;
        }
    }

    public async Task<bool> SerializeObjectToJsonFileAsync<T>(T obj, string directory, string fileName)
    {
        string filePath = Path.Combine(directory, fileName);
        try
        {
            using MemoryStream memoryStream = new();
            DataContractJsonSerializer serializer = new(typeof(T));
            serializer.WriteObject(memoryStream, obj);
            Directory.CreateDirectory(directory);
            File.WriteAllText(filePath, Encoding.UTF8.GetString(memoryStream.ToArray()));
            return true;
        }
        catch (Exception ex)
        {
            _fileLog.AppendLine(ex.ToString());
            await _alert?.DisplayAlertAsync("Error", $"JSON serialization error: {ex.Message}", "Ok");
            return false;
        }
    }
}
