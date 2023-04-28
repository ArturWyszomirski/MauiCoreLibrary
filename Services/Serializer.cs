namespace MauiCoreLibrary.Services
{
    public class Serializer : ISerializer
    {
        private readonly IFileLogService _fileLog;

        public Serializer(IFileLogService fileLog)
        {
            _fileLog = fileLog;
        }

        public T DeserializeObjectFromJsonFile<T>(string directory, string fileName)
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
                _fileLog.AppendLine($"JSON deserialization error: {ex.Message}");
                return default;
            }
        }

        public bool SerializeObjectToJsonFile<T>(T obj, string directory, string fileName)
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
                _fileLog.AppendLine($"JSON serialization error: {ex.Message}");
                return false;
            }
        }
    }
}
