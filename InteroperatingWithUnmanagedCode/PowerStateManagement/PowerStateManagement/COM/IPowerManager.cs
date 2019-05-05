using System.Runtime.InteropServices;

namespace PowerStateManagementLibrary.COM
{
    [ComVisible(true)]
    [Guid("92B6CE42-F027-4E00-9CF3-C4F1873B0D87")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPowerManager
    {
        long GetLastSleepTime();
        long GetLastWakeTime();
    }
}