using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;

namespace MauiCoreLibrary.Helpers;

public class Onnx
{
    /// <summary>
    /// ONNX file should be put in the ../Resources/Raw directory.
    /// </summary>
    /// <returns></returns>
    public static async Task<InferenceSession> LoadOnnxAsync(string fileName)
    {
        using Stream stream = await FileSystem.OpenAppPackageFileAsync(fileName);

        MemoryStream memoryStream = new(); // Need to be done with MemoryStream as Stream.Length and Stream.Position are not supported under Android. 
        stream.CopyTo(memoryStream);

        int streamLength = memoryStream.GetBuffer().Length;
        byte[] buffer = new byte[streamLength];
        memoryStream.Position = 0;
        memoryStream.Read(buffer, 0, buffer.Length);

        InferenceSession session = new(buffer);

        return session;
    }

    public static List<NamedOnnxValue> CreateInputs(List<float[]> data, int numberOfColumns, int numberOfRows)
    {
        ReadOnlySpan<int> dimensions = new int[] { numberOfRows }.Concat(new int[] { numberOfColumns }).ToArray();
        DenseTensor<float> inputTensor = new(dimensions);

        int dataRowNumber = 0;
        foreach (float[] dataRow in data)
        {
            for (int i = 0; i < dataRow.Length; i++)
                inputTensor.SetValue(i + (dataRow.Length * dataRowNumber), dataRow[i]);
            dataRowNumber++;
        }

        List<NamedOnnxValue> inputs = new() { NamedOnnxValue.CreateFromTensor("X", inputTensor) };

        return inputs;
    }
}
