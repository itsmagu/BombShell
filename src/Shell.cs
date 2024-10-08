﻿using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace BombShell.ShellScope;

public partial class Shell : Control, IShell
{
    // Godot Properties
    [Export] public ColorRect Bg = null!;             // Export Attribute will expose
    [Export] public LineEdit CommandLine = null!;     // these to Godot. So they are
    [Export] public RichTextLabel ConsoleLog = null!; // not null when loaded via Godot
    public Queue<string> CommandQueue { get; } = new Queue<string>();
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
                CommandQueue.Enqueue(CommandLine.Text);
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
    public static Shell Instantiate(){
        return GD.Load<PackedScene>("res://scn/shell.tscn").Instantiate<Shell>();
    }
    public void PrintToShell(string message){
        ConsoleLog.Text += $"\n{message}";
    }
}