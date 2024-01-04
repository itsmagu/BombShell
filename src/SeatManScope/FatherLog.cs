using Godot;

namespace BombShell.SeatManScope;

public partial class FatherLog : Control
{
    [Export] private RichTextLabel log = null!;
    public void Send(string caller,string message){
        log.Text += $"\n[{caller}] {message}";
    }
    public static FatherLog Instantiate(){
        return GD.Load<PackedScene>("res://scn/fatherLog.tscn").Instantiate<FatherLog>();
    }
}