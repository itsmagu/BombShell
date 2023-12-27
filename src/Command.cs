using System.Collections.Generic;
using Godot;

namespace BombShell;

public partial class MainNode
{
    private abstract class Command
    {
        public abstract void Run(Node owner);
    }

    private class Ping : Command
    {
        public override void Run(Node owner){
            owner.Rpc(MethodName.Pong, 5);
        }
    }

    private class Exit : Command
    {
        public override void Run(Node owner){
            owner.GetTree().Quit();
        }
    }

    private readonly List<Command> commands = [
        new Exit(),
        new ServerMe(),
        new ClientMe(),
        new Ping()
    ];
}