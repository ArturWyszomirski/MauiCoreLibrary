using System.Runtime.InteropServices;

namespace MauiCoreLibrary.Services;

public class PowerManagementService : IPowerManagementService
{
    public bool CpuSleepDisabled { get; private set; }

    public void DisableCpuSleep()
    {
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        CpuSleepDisabled = true;
    }

    public void EnableCpuSleep()
    {
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        CpuSleepDisabled = false;
    }

    /// <summary>
    /// Set execution state (disable sleep, turning off display).
    /// </summary>
    /// <param name="esFlags"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
}
