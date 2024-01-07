using System.Collections.Generic;
using BombShell.EmuSystemScope;
using Godot;

namespace BombShell;

public partial class Game : Node
{
    public static List<EmuSystem> ClientMachines { get; } = [];
    public static ulong GameTick { get; private set; } = 0;
    public override void _PhysicsProcess(double delta){
        foreach (EmuSystem clientMachine in ClientMachines){
            clientMachine.Process(GameTick);
        }
        GameTick++;
    }
}