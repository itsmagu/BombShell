using System.Collections.Generic;

namespace BombShell.EmuSystemScope;

public class EmuSystem
{
    //Statics
    public static List<EmuSystem> EmuSystems = new();

    //Properties
    public EmuState EmuState { get; private set; } = EmuState.Offline;
    public List<User> Users { get; } = [new User("root", true)];
    public SeatManScope.FatherLog? ConnectedFatherLog = null!;
    public FileSystem? FileSystem { get; set; }

    //Methods
    public string Boot(){ //TODO Async this
        if (EmuState != EmuState.Offline) return "Already Online!";
        // File System
        if (FileSystem == null) return "Could not find a valid filesystem...";
        if (FileSystem.Used) return "Found Filesystem is currently Used";
        if (!FileSystem.Root.Files.ContainsKey("boot"))
            return "Could not find a valid boot file...";
        FileSystem.Used = true;
        ConnectedFatherLog?.Send($"Tree:{FileSystem.Root.ListAllRecursive(5)}");
        //Boot Process
        EmuState = EmuState.Active;
        return "Booted!";
    }
}

public enum EmuState
{
    Offline,
    Active,
}