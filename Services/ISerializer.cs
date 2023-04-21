namespace MauiCoreLibrary.Services
{
    public interface ISerializer
    {
        T DeserializeObjectFromJsonFile<T>(string directory, string fileName);
        bool SerializeObjectToJsonFile<T>(T obj, string directory, string fileName);
    }
}