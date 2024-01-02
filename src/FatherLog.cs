using Godot;

namespace BombShell;

public partial class FatherLog : Control
{
    [Export] private RichTextLabel log = null!;
    public void Send(string message){
        log.Text += $"\n{message}";
    }
}