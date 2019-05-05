using System;
using System.Runtime.InteropServices;

namespace PowerStateManagementLibrary
{
    public class PowerManagement
    {
        [DllImport("PowrProf.dll", EntryPoint = "CallNtPowerInformation", ExactSpelling = true, CharSet = CharSet.Auto,
            SetLastError = true)]
        private static extern int CallNtPowerInformation(
            Int32 informationLevel,
            [In] IntPtr lpInputBuffer,
            uint nInputBufferSize,
            [In, Out] IntPtr lpOutputBuffer,
            uint nOutputBufferSize);

        /// <summary>
        /// Suspends the system by shutting power down. Depending on the Hibernate parameter, the system either enters a suspend (sleep) state or hibernation (S4).
        /// </summary>
        /// <param name="hibernate">If this parameter is TRUE, the system hibernates. If the parameter is FALSE, the system is suspended.</param>
        /// <param name="forceCritical">Windows Server 2003, Windows XP, and Windows 2000:  If this parameter is TRUE, 
        /// the system suspends operation immediately; if it is FALSE, the system broadcasts a PBT_APMQUERYSUSPEND event to each 
        /// application to request permission to suspend operation.</param>
        /// <param name="disableWakeEvent">If this parameter is TRUE, the system disables all wake events. If the parameter is FALSE, any system wake events remain enabled.</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("Powrprof.dll", SetLastError = true)]
        static extern uint SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        public static void MachineSleep(bool hibernate)
        {
            SetSuspendState(hibernate, false, false);
        }

        public static long GetLastActivity(PowerInformationLvl informationLvl)
        {
            var buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(long)));

            CallNtPowerInformation((int) informationLvl, IntPtr.Zero, 0, buffer, (uint) Marshal.SizeOf(typeof(long)));

            Marshal.FreeCoTaskMem(buffer);

            var lastSleepTimeInSeconds = Marshal.ReadInt64(buffer, 0) / 10000000;

            return lastSleepTimeInSeconds;
        }

        public static SystemBatteryState GetBatteryState()
        {
            var size = Marshal.SizeOf<SystemBatteryState>();

            var batteryStatePtr = Marshal.AllocCoTaskMem(size);

            CallNtPowerInformation(5, IntPtr.Zero, 0, batteryStatePtr, (uint) size);

            Marshal.FreeCoTaskMem(batteryStatePtr);

            return Marshal.PtrToStructure<SystemBatteryState>(batteryStatePtr);
        }

        public static SystemPowerInformation GetPowerInfo()
        {
            var size = Marshal.SizeOf<SystemBatteryState>();

            var powerInfoPtr = Marshal.AllocCoTaskMem(size);

            CallNtPowerInformation(12, IntPtr.Zero, 0, powerInfoPtr, (uint)size);

            Marshal.FreeCoTaskMem(powerInfoPtr);

            return Marshal.PtrToStructure<SystemPowerInformation>(powerInfoPtr);
        }

        /// <summary>
        /// If lpInBuffer is not NULL and the current user has sufficient privileges, the function commits or decommits the storage required
        /// to hold the hibernation image on the boot volume. The lpInBuffer parameter must point to a BOOLEAN value indicating the desired request. 
        /// If the value is TRUE, the hibernation file is reserved; if the value is FALSE, the hibernation file is removed.
        /// </summary>
        public static void ReserveHiberFile(bool flag)
        {
            var flagAsByte = Convert.ToByte(flag);

            var unmanagedPointer = Marshal.AllocCoTaskMem(sizeof(byte));
            Marshal.WriteByte(unmanagedPointer, flagAsByte);

            CallNtPowerInformation(10, unmanagedPointer, (uint)Marshal.SizeOf<byte>(), IntPtr.Zero, 0);

            Marshal.FreeCoTaskMem(unmanagedPointer);
        }

        public enum PowerInformationLvl
        {
            LastWakeTime = 14,
            LastSleepTime = 15
        }

        public struct SystemPowerInformation
        {
            public uint MaxIdlenessAllowed;
            public uint Idleness;
            public uint TimeRemaining;
            public byte CoolingMode;
        }

        public struct SystemBatteryState
        {
            public byte AcOnLine;
            public byte BatteryPresent;
            public byte Charging;
            public byte Discharging;
            public byte spare1;
            public byte spare2;
            public byte spare3;
            public byte spare4;
            public UInt32 MaxCapacity;
            public UInt32 RemainingCapacity;
            public Int32 Rate;
            public UInt32 EstimatedTime;
            public UInt32 DefaultAlert1;
            public UInt32 DefaultAlert2;
        }
    }
}