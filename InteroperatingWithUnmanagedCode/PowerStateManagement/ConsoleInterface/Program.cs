using System;
using PowerStateManagementLibrary;
#pragma warning disable 1587

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            var lastSleepTime = PowerManagement.GetLastActivity(PowerManagement.PowerInformationLvl.LastSleepTime);
            var lastWakeTime = PowerManagement.GetLastActivity(PowerManagement.PowerInformationLvl.LastWakeTime);
            var batteryState = PowerManagement.GetBatteryState();
            var powerInfo = PowerManagement.GetPowerInfo();
            Console.WriteLine($"Last sleep time: {lastSleepTime} seconds.");
            Console.WriteLine($"Last wake time: {lastWakeTime} seconds.");
            Console.WriteLine($"Battery level: {batteryState.RemainingCapacity}/{batteryState.MaxCapacity}");
            Console.WriteLine($"Remaining time: {powerInfo.TimeRemaining} | Cooling mode: {powerInfo.CoolingMode} " +
                              $"| Idleness: {powerInfo.Idleness} | Max Idleness allowed: {powerInfo.MaxIdlenessAllowed}");

            // PowerManagement.ReserveHiberFile(true);
            /// <param name="hibernate">If this parameter is TRUE, the system hibernates. If the parameter is FALSE, the system is suspended.</param>
            //PowerManagement.MachineSleep(false);
        }
    }
}
