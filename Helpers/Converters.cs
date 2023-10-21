namespace MauiCoreLibrary.Helpers;
public class Converters
{
    public static byte[] StreamToBytes(Stream stream)
    {
        using MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
