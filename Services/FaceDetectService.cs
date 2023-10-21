using NcnnDotNet.OpenCV;
using UltraFaceDotNet;
using NcnnDotNet;

namespace MauiCoreLibrary.Services;

/// <summary>
/// RFB-320.bin and RFB-320.param files should be copied to project's Resources/Raw assets directory.
/// Based on https://xamaringuyshow.com/2023/07/30/dotnet-maui-open-cv-face-detection/
/// </summary>
public class FaceDetectService : IFaceDetectService
{
    #region Fields
    private readonly UltraFace _ultraFace;
    #endregion

    #region Constructors
    public FaceDetectService()
    {
        var files = new[] { "RFB-320.bin", "RFB-320.param" };

        foreach (var file in files)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);
            using var fs = File.Create(path);
            using var stream = FileSystem.OpenAppPackageFileAsync(file).Result;
            stream.CopyTo(fs);
        }

        var binPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), files[0]);
        var paramPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), files[1]);

        var param = new UltraFaceParameter
        {
            BinFilePath = binPath,
            ParamFilePath = paramPath,
            InputWidth = 320,
            InputLength = 240,
            NumThread = 1,
            ScoreThreshold = 0.7f
        };

        _ultraFace = UltraFace.Create(param);
    }
    #endregion

    #region Public methods
    public DetectResult Detect(byte[] file)
    {
        using var frame = Cv2.ImDecode(file, CvLoadImage.Grayscale);
        if (frame.IsEmpty)
            throw new NotSupportedException("This file is not supported!!");

        if (Ncnn.IsSupportVulkan)
            Ncnn.CreateGpuInstance();

        using var inMat = NcnnDotNet.Mat.FromPixels(frame.Data, NcnnDotNet.PixelType.Bgr2Rgb, frame.Cols, frame.Rows);

        var faceInfos = this._ultraFace.Detect(inMat).ToArray();

        if (Ncnn.IsSupportVulkan)
            Ncnn.DestroyGpuInstance();

        return new DetectResult(frame.Cols, frame.Rows, faceInfos);
    }
    #endregion

    public sealed class DetectResult
    {
        #region Constructors
        public DetectResult(int width, int height, IEnumerable<FaceInfo> boxes)
        {
            Width = width;
            Height = height;
            Boxes = new List<FaceInfo>(boxes);
        }
        #endregion

        #region Properties
        public IReadOnlyCollection<FaceInfo> Boxes
        {
            get;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }
        #endregion
    }
}

