using BombShell.EmuSystemScope.Filesystem;
using BombShell.SeatManScope;
using BombShell.ShellScope;

namespace BombShell.EmuSystemScope;

public class EmuSystem
{
    //Properties
    public EmuState EmuState { get; private set; } = EmuState.Offline;
    public FatherLog? connectedFatherLog = null;
    public IShell? connectedShell = null;
    public EmuFileSystem? FileSystem { get; init; }

    //Methods
    // Boot() this should make sure that it can bind and then start the bootloader
    public string BindAndBoot(){
        return Boot();
    }
    public string Boot(){ //TODO Async this maybe
        if (EmuState != EmuState.Offline) return "Already Online!";
        // File System
        if (FileSystem == null) return "Could not find a valid filesystem...";
        if (FileSystem.Used) return "Found Filesystem is currently Used";
        // Find a Boot Configuration File
        if (!FileSystem.RootContent.Files.ContainsKey("boot"))
            return "Could not find a valid boot file...";
        FileSystem.Used = true;
        connectedFatherLog?.SendToLog(
            this,
            $"Loaded this Filesystem:{FileSystem.RootContent.ListAllRecursive(5)}"
        );
        // Boot Process
        EmuState = EmuState.Active;
        return "Booted!";
    }
    public void Process(ulong gameTick){ }
}

public enum EmuState
{
    Offline,
    Active,
}