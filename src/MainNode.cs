using System.Collections.Generic;
using Godot;

namespace BombShell;

public partial class MainNode : Node
{
    [Export] private Control seatWindow = null!;
    public static List<Control> ListOfSeats = new List<Control>();
    private static int currentSelectedSeat = 0;
    public static FatherLog MainFatherLog { get; private set; } = null!;
    public static int CurrentSelectedSeat {
        get => currentSelectedSeat;
        private set {
            for (int i = 0; i < ListOfSeats.Count; i++){
                Control control = ListOfSeats[i];
                if (i == value){
                    control.Visible = true;
                    control.ProcessMode = ProcessModeEnum.Inherit;
                } else{
                    control.Visible = false;
                    control.ProcessMode = ProcessModeEnum.Disabled;
                }
            }
            currentSelectedSeat = value;
        }
    }
    public override void _Ready(){
        FatherLog fatherLog = GD.Load<PackedScene>("res://scn/fatherLog.tscn")
            .Instantiate<FatherLog>();
        MainFatherLog = fatherLog;
        AddSeat(fatherLog);
        AddSeat(GD.Load<PackedScene>("res://scn/shell.tscn").Instantiate<Control>());
        EmuSystem newSystem = new EmuSystem {
            FileSystem = new EmuFileSystem() {
                Root = {
                    Files = {
                        {
                            "boot", new File("cool item")
                        }
                    }
                }
            },
            ConnectedFatherLog = fatherLog
        };
        EmuSystem.EmuSystems.Add(new EmuSystem());
        MainFatherLog.Send(newSystem.Boot());
    }
    public void AddSeat(Control newWindow){
        newWindow.Visible = false;
        newWindow.ProcessMode = ProcessModeEnum.Disabled;
        ListOfSeats.Add(newWindow);
        seatWindow.AddChild(newWindow, true);
        CurrentSelectedSeat = ListOfSeats.Count - 1;
    }
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
}