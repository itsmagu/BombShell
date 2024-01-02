using System.Collections.Generic;
using Godot;

namespace BombShell.SeatManScope;

public partial class SeatManager : Control
{
    [Export] private Control seatHandle = null!;
    [Export] private Control machineTab = null!;
    // List of Seats
    public readonly List<Control> ListOfSeats = [];
    // Current Seat
    public int CurrentSelectedSeat;
    // Add Seat
    public void AddSeat(Control newWindow){
        newWindow.Visible = false;
        newWindow.ProcessMode = ProcessModeEnum.Disabled;
        ListOfSeats.Add(newWindow);
        seatHandle.AddChild(newWindow, true);
    }
    // Override
    public override void _Input(InputEvent @event){
        if (@event is InputEventKey { Pressed: true } eventKey){
            switch (eventKey.Keycode){
            case Key.F1:
                if (ListOfSeats.Count < 2) break;
                if (CurrentSelectedSeat == ListOfSeats.Count - 1)
                    CurrentSelectedSeat = 0;
                else
                    CurrentSelectedSeat++;
                break;
            default: return;
            }
            GetViewport().SetInputAsHandled();
        }
    }
    public override void _Ready(){
        AddSeat(GD.Load<PackedScene>("res://scn/fatherLog.tscn").Instantiate<Control>());
        AddSeat(GD.Load<PackedScene>("res://scn/shell.tscn").Instantiate<Control>());
    }
}
