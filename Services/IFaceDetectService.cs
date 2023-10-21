namespace MauiCoreLibrary.Services;

public interface IFaceDetectService
{
    FaceDetectService.DetectResult Detect(byte[] file);
}