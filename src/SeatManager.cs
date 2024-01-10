using System;
using System.Collections.Generic;
using Godot;

namespace BombShell.SeatManScope;

public partial class SeatManager : Control
{
    [Export] public Control seatHandle = null!;
    [Export] public Control seatTab = null!;
    // Seats and Seat Manipulation
    public static List<Control> ListOfSeats { get; } = [];
    private int _currentSelectedSeat;
    public int CurrentSelectedSeat {
        get => _currentSelectedSeat;
        set {
            // If the list is 1 or 0 set to 0
            int l = ListOfSeats.Count;
            if (l < 2){
                _currentSelectedSeat = 0;
            } // If we are over the count of Seats we loop
            else if (value >= l)
                _currentSelectedSeat = value % l;
            // If we are under 0 then loop from count of seats downwards
            else if (value < 0)
                _currentSelectedSeat = l - (Math.Abs(value) % l);
            else
                // Otherwise just set
                _currentSelectedSeat = value;
            // Trigger Side effects
            for (int i = 0; i < ListOfSeats.Count; i++){
                Control control = ListOfSeats[i];
                if (i == _currentSelectedSeat){
                    control.Visible = true;
                    control.ProcessMode = ProcessModeEnum.Inherit;
                    if (seatTab.GetChildCount() == l)
                        seatTab.GetChild<Control>(i).SelfModulate = Colors.DarkRed;
                } else{
                    control.Visible = false;
                    control.ProcessMode = ProcessModeEnum.Disabled;
                    if (seatTab.GetChildCount() == l)
                        seatTab.GetChild<Control>(i).SelfModulate = Colors.White;
                }
            }
        }
    }
    // Add Seat
    public void AddSeat(Control newWindow){
        newWindow.Visible = false;
        newWindow.ProcessMode = ProcessModeEnum.Disabled;
        ListOfSeats.Add(newWindow);
        seatHandle.AddChild(newWindow, true);
    }
    public void RebuildTabs(){
        foreach (Node child in seatTab.GetChildren()){
            child.Free();
        }
        for (int i = 0; i < ListOfSeats.Count; i++){
            Control control = ListOfSeats[i];
            seatTab.AddChild(
                new Label() {
                    Text = control.Name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    SizeFlagsHorizontal = SizeFlags.Expand,
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
                CurrentSelectedSeat--;
                break;
            case Key.F2:
                CurrentSelectedSeat++;
                break;
            default: return;
            }
            GetViewport().SetInputAsHandled();
        }
    }
}