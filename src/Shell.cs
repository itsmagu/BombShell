using System;
using Godot;

namespace BombShell;

public partial class Shell : Control
{
    // Godot Properties
    [Export] public ColorRect Bg = null!;             // Export Attribute will expose
    [Export] public LineEdit CommandLine = null!;     // these to Godot. So they are
    [Export] public RichTextLabel ConsoleLog = null!; // not null when loaded via Godot
    public override void _Ready(){
        // Randomize Terminal Color
        Bg.Color = new Color(
            Random.Shared.NextSingle(),
            Random.Shared.NextSingle(),
            Random.Shared.NextSingle(),
            .3f
        );
        // Startup Message
        ConsoleLog.Text =
            "Welcome Netrunner, a VM has been setup on this machine for your protection!\n" +
            "A connection to your VM is open from this window, Good Diving ;)" +
            "\nThis game is made to explore how computers work and how to manipulate them." +
            "\nIn this window(s) no harm can actually be done since it is all just a game";
        // Focus the shell
        CommandLine.GrabFocus();
    }
    public override void _Input(InputEvent @event){
        if (@event is InputEventKey { Pressed: true } eventKey){
            switch (eventKey.Keycode){
            case Key.Enter:
                ConsoleLog.Text += $"\n>{CommandLine.Text}";
                CommandLine.Text = "";
                break;
            case Key.Escape:
                CommandLine.Text = "";
                break;
            case Key.F5:
                ConsoleLog.Text = "~Reset Console";
                break;
            default: return;
            }
            GetViewport().SetInputAsHandled();
        }
    }
}

interface IShell
{
    void Out();
    void In();
}