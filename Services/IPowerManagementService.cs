namespace MauiCoreLibrary.Services;

public interface IPowerManagementService
{
    bool CpuSleepDisabled { get; }

    /// <summary>
    /// Prevents device's CPU from going to sleep mode or killing app due to energy saving (Android).
    /// </summary>
    void DisableCpuSleep();

    /// <summary>
    /// Enable device's CPU going sleep mode or killing app due to energy saving (Android).
    /// </summary>
    void EnableCpuSleep();
}
