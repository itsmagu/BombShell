using System.Collections.Generic;
using BombShell.EmuSystemScope;
using BombShell.EmuSystemScope.Filesystem;
using BombShell.SeatManScope;
using BombShell.ShellScope;
using Godot;

namespace BombShell;

public static class Main
{
    public static List<EmuSystem> ClientMachines { get; } = [];
}

public partial class MainNode : Node
{
    public static ulong GameTick { get; private set; } = 0;
    public override void _PhysicsProcess(double delta){
        foreach (EmuSystem clientMachine in Main.ClientMachines){
            clientMachine.Process(GameTick);
        }
        ++GameTick;
    }
    public override void _Ready(){
        SeatManager seatManager = GD.Load<PackedScene>("res://scn/seatMan.tscn")
            .Instantiate<SeatManager>();
        AddChild(seatManager);
    }
}