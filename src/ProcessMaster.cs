using Godot;

namespace BombShell;

public partial class ProcessMaster : Node
{
    public static ulong GameTick { get; private set; } = 0;
    public override void _PhysicsProcess(double delta){
        GD.Print($"{GameTick} = {Engine.GetPhysicsFrames()}");
        GameTick++;
    }
}