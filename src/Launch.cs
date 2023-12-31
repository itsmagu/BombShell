using Godot;

namespace BombShell;

public partial class Launch : Control
{
    [Export] private Button ServerMe = null!;
    [Export] private Button ClientMe = null!;
    [Export] private Button MachineMe = null!;
    [Export] private Button ConnectMe = null!;
    public override void _Ready(){
        ServerMe.Pressed += () => {
            
        };
        ClientMe.Pressed += () => {
            GetParent().AddChild(GD.Load<PackedScene>("res://scn/shell.tscn").Instantiate());
            QueueFree();
        };
        MachineMe.Pressed += () => {
            
        };
        ConnectMe.Pressed += () => {
            
        };
    }
}