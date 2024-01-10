using System.Collections.Generic;
using BombShell.EmuSystemScope.Filesystem;
using BombShell.EmuSystemScope.Software;
using BombShell.SeatManScope;
using BombShell.ShellScope;
using Godot;
using Laylua.Library;

namespace BombShell.EmuSystemScope;

public class EmuSystem
{
    //Properties
    public EmuState EmuState { get; private set; } = EmuState.Offline;
    public FatherLog? ConnectedFatherLog { get; set; } = null;
    public IShell? ConnectedShell { get; set; } = null;
    public EmuFileSystem? FileSystem { get; init; }
    public SortedDictionary<uint, Process> ProcessQue { get; } = new() {
        { 1, new Process(Proc, new User("root", true)) }
    };
    private static void Proc(ulong tick, IShell? shell, Lua lua){
        if (shell?.CommandQueue.Count > 0)
            for (int i = 0; i < shell!.CommandQueue.Count; i++){
                string cmd = shell.CommandQueue.Dequeue();
                shell?.PrintToShell($"tick {tick} runs \"{cmd}\" in the lua machine");
                try{
                    lua.Execute(cmd);
                } catch (LuaException e){
                    shell!.PrintToShell(e.Message);
                }
            }
    }

    //Methods
    public void Process(ulong gameTick){
        ConnectedFatherLog?.SendToLog("processor", gameTick.ToString());
        GD.Print($"Machine {Main.ClientMachines.IndexOf(this)} got a tick");
        foreach (Process process in ProcessQue.Values)
            process.Proc(gameTick, ConnectedShell, process.LuaMachine);
    }
    // Boot() this should make sure that it can bind and then start the bootloader
    public string Boot(){ //TODO Async this maybe
        if (EmuState != EmuState.Offline) return "Already Online!";
        // File System
        if (FileSystem == null) return "Could not find a valid filesystem...";
        if (FileSystem.Used) return "Found Filesystem is currently Used";
        // Find a Boot Configuration File
        if (!FileSystem.RootContent.Files.ContainsKey("boot"))
            return "Could not find a valid boot file...";
        // Set the Filesystem to used
        FileSystem.Used = true;
        // Boot Process
        EmuState = EmuState.Active;
        ConnectedShell?.PrintToShell("This shell is connected to a machine!");
        return "Booted System!";
    }
}

public enum EmuState
{
    Offline,
    Active,
}