using System.Collections.Generic;

namespace BombShell;

public class EmuSystem
{
    //Statics
    public static List<EmuSystem> EmuSystems = new();

    //Properties
    public EmuState EmuState { get; private set; } = EmuState.Offline;
    public EmuFileSystem? FileSystem { get; set; }

    //Methods
    public string Boot(){
        if (EmuState != EmuState.Offline) return "Already Online!";
        if (FileSystem == null || FileSystem.Root.Content[0].Name != "boot")
            return "Could not find a valid filesystem with a boot file...";
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