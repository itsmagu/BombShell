using BombShell.EmuSystemScope;
using BombShell.EmuSystemScope.Filesystem;
using BombShell.SeatManScope;
using BombShell.ShellScope;
using Godot;

namespace BombShell;

public partial class MainNode : Node
{
    public override void _Ready(){
        // Load SeatMan
        SeatManager seatManager = GD.Load<PackedScene>("res://scn/seatMan.tscn")
            .Instantiate<SeatManager>();
        AddChild(seatManager);
        // Load a FL for our first Machine
        FatherLog fatherLog = FatherLog.Instantiate();
        // Shell
        Shell shell = Shell.Instantiate();
        // Create our first Machine
        EmuSystem emuSystem = new EmuSystem() {
            connectedFatherLog = fatherLog,
            connectedShell = shell,
            FileSystem =
                new EmuFileSystem() { RootContent = EmuFileSystem.StandardFileSystem() }
        };
        Game.ClientMachines.Add(emuSystem);
        // Add the FL to the SeatMan
        seatManager.AddSeat(fatherLog);
        seatManager.AddSeat(shell);
        seatManager.CurrentSelectedSeat = 0;
        seatManager.RebuildTabs();
        // Boot the System and return to FL with the log
        fatherLog.SendToLog(this, emuSystem.Boot());
    }
}