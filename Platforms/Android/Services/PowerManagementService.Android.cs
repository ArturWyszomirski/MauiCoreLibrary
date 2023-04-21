using Android.Content;
using Android.OS;

namespace MauiCoreLibrary.Services;

public class PowerManagementService : IPowerManagementService
{
    private readonly PowerManager.WakeLock _wakeLock;

    public PowerManagementService()
    {
        PowerManager powerManager = Android.App.Application.Context.GetSystemService(Context.PowerService) as PowerManager;
        _wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, "ServiceWakeLock");
    }

    public bool CpuSleepDisabled { get; private set; }

    public void DisableCpuSleep()
    {
        _wakeLock?.Acquire();
        CpuSleepDisabled = true;
    }

    public void EnableCpuSleep()
    {
        _wakeLock?.Release();
        CpuSleepDisabled = false;
    }
}
