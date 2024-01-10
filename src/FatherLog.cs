using BombShell.EmuSystemScope;
using Godot;

namespace BombShell.SeatManScope;

public partial class FatherLog : Control
{
    [Export] private RichTextLabel _log = null!;
    public void SendToLog(object source, string message){
        _log.Text += $"\n[{source}] {message}";
    }
    public static FatherLog Instantiate(){
        return GD.Load<PackedScene>("res://scn/fatherLog.tscn").Instantiate<FatherLog>();
    }
    public static void InitAndBind(EmuSystem emuSystem){
        var self = Instantiate();
        emuSystem.ConnectedFatherLog = self;
    }
}