using System;
using System.Collections.Generic;
using Godot;

namespace BombShell;

public partial class SeatManager : Control
{
    [Export] private Control seatHandle = null!;
    [Export] private Control machineTab = null!;
    // List of Seats
    public readonly List<Control> ListOfSeats = new List<Control>();
    // Current Seat
    private int currentSelectedSeat;
    public int CurrentSelectedSeat {
        get => currentSelectedSeat;
        private set {
            value = Math.Max(value, 0);
            for (int i = 0; i < ListOfSeats.Count; i++){
                Control control = ListOfSeats[i];
                if (i == value){
                    control.Visible = true;
                    control.ProcessMode = ProcessModeEnum.Inherit;
                    if (machineTab.GetChildCount() > 0)
                        machineTab.GetChild<Control>(i).SelfModulate = Colors.DarkRed;
                } else{
                    control.Visible = false;
                    control.ProcessMode = ProcessModeEnum.Disabled;
                    if (machineTab.GetChildCount() > 0)
                        machineTab.GetChild<Control>(i).SelfModulate = Colors.White;
                }
            }
            currentSelectedSeat = value;
        }
    }
    // Add Seat
    public void AddSeat(Control newWindow){
        newWindow.Visible = false;
        newWindow.ProcessMode = ProcessModeEnum.Disabled;
        ListOfSeats.Add(newWindow);
        seatHandle.AddChild(newWindow, true);
        BuildSeatUi();
    }
    // View Seats
    public void BuildSeatUi(){
        foreach (Node child in machineTab.GetChildren()){
            child.QueueFree();
        }
        for (int i = 0; i < Math.Max(ListOfSeats.Count,0); i++){
            Control seat = ListOfSeats[i];
            machineTab.AddChild(
                new Label() {
                    Text = seat.Name,
                    SizeFlagsHorizontal = SizeFlags.ExpandFill,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    SelfModulate = i == CurrentSelectedSeat ? Colors.DarkRed : Colors.White
                }
            );
        }
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
