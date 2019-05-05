Module Module1
    'In directory of current project there are reg.cmd and unreg.cmd files. Before testing usage of COM 
    'from VB, register it with this files and then unregister.
    Sub Main()
        Dim obj = CreateObject("PowerStateManagementLibrary.COM.PowerManager")
        Console.WriteLine($"Sleep time: {obj.GetLastSleepTime()} seconds.")
        Console.WriteLine($"Wake time: {obj.GetLastWakeTime()} seconds.")
    End Sub

End Module
