using Godot;

namespace BombShell.SeatManScope;

public partial class FatherLog : Control
{
    [Export] private RichTextLabel log = null!;
    public void SendToLog(object source, string message){
        log.Text += $"\n[{source}] {message}";
    }
    public static FatherLog Instantiate(){
        return GD.Load<PackedScene>("res://scn/fatherLog.tscn").Instantiate<FatherLog>();
    }
}