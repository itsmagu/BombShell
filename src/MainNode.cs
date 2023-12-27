using System;
using System.Linq;
using Godot;

namespace BombShell;

public partial class MainNode : Node
{
    [Export] public ColorRect Bg;
    [Export] public LineEdit Shell;
    [Export] public RichTextLabel Log;
    private const uint MAX_HISTORY = 10;
    public override void _Ready(){
        // Randomize Terminal Color
        Bg.Color = new Color(
            Random.Shared.NextSingle(),
            Random.Shared.NextSingle(),
            Random.Shared.NextSingle(),
            .3f
        );
        // Startup Message
        GD.Print("Booted!");
        Log.Text =
            "Welcome Netrunner, a VM has been setup on this machine for your protection!\n" +
            "A connection to your VM is open from this window, Good Diving ;)" +
            "\nThis game is made to explore how computers work and how to manipulate them." +
            "\nIn this window(s) no harm can actually be done since it is all just a game";
        // Focus the shell
        Shell.GrabFocus();
    }
    public override void _PhysicsProcess(double delta){
        // TODO Processes
    }
    public override void _Input(InputEvent @event){
        if (@event is InputEventKey { Pressed: true } eventKey)
            switch (eventKey.Keycode){
            case Key.Enter:
                Log.Text += $"\n>{Shell.Text}";
                RunCommand(Shell.Text);
                Shell.Text = "";
                break;
            case Key.Escape:
                Shell.Text = "";
                GetViewport().SetInputAsHandled();
                break;
            case Key.F5:
                Log.Text = "~Reset Console";
                break;
            case Key.F2:
                RunCommand("ServerMe");
                break;
            case Key.F1:
                RunCommand("ClientMe");
                break;
            }
    }
    private void RunCommand(string commandString){
        foreach (Command command in commands.Where(
                command => command.GetType().Name.Equals(
                    commandString,
                    StringComparison.CurrentCultureIgnoreCase
                )
            )){
            Log.Text += $"\nEXEC @ {command.GetType().Name}";
            command.Run(this);
            break;
        }
    }

    private class ServerMe : Command //TODO Make me a ProcessSystem Pweese
    {
        public override void Run(Node owner){
            if (owner.Multiplayer.MultiplayerPeer.GetType() !=
                typeof(OfflineMultiplayerPeer)) return;
            WebSocketMultiplayerPeer netPeer = new WebSocketMultiplayerPeer();
            if (netPeer.CreateServer(5270) == Error.Ok){
                owner.Multiplayer.MultiplayerPeer = netPeer;
                owner.Multiplayer.PeerConnected += id => {
                    GD.Print($"{id} connected");
                    int cool = Random.Shared.Next();
                    GD.Print("Sent " + cool);
                    owner.RpcId(id, MethodName.Pong, cool);
                };
                GD.Print("SERVER");
            } else{
                GD.Print("ERROR");
            }
        }
    }

    [Rpc(
        MultiplayerApi.RpcMode.AnyPeer,
        TransferMode = MultiplayerPeer.TransferModeEnum.Reliable,
        CallLocal = false
    )]
    private void Pong(int pong){
        Log.Text += $"\npong {pong}";
    }

    private class ClientMe : Command
    {
        public override void Run(Node owner){
            if (owner.Multiplayer.MultiplayerPeer.GetType() !=
                typeof(OfflineMultiplayerPeer)) return;
            WebSocketMultiplayerPeer netPeer = new WebSocketMultiplayerPeer();
            if (netPeer.CreateClient("ws://localhost:5270") == Error.Ok){
                owner.Multiplayer.MultiplayerPeer = netPeer;
                GD.Print("CLIENT");
            } else{
                GD.Print("ERROR");
            }
        }
    }
}