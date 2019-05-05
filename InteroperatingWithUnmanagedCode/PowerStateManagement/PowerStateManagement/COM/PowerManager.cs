using System.Runtime.InteropServices;

namespace PowerStateManagementLibrary.COM
{
    // In directory of current project there are reg.cmd and unreg.cmd files. Before testing usage of COM 
    // from VB, register it with this files and then unregister.
    [ComVisible(true)]
    [Guid("CE50E6DA-217F-4300-971A-86C81381BFEC")]
    [ClassInterface(ClassInterfaceType.None)]
    public class PowerManager : IPowerManager
    {
        public long GetLastSleepTime()
        {
            return PowerManagement.GetLastActivity(PowerManagement.PowerInformationLvl.LastSleepTime);
        }

        public long GetLastWakeTime()
        {
            return PowerManagement.GetLastActivity(PowerManagement.PowerInformationLvl.LastWakeTime);
        }
    }
}