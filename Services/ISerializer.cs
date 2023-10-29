namespace MauiCoreLibrary.Services
{
    public interface ISerializeService
    {
        Task<T> DeserializeObjectFromJsonFileAsync<T>(string directory, string fileName);
        Task<bool> SerializeObjectToJsonFileAsync<T>(T obj, string directory, string fileName);
    }
}