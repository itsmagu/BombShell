using System.Collections.Generic;
using Godot;

namespace BombShell;

public partial class MainNode
{
    private abstract class Command
    {
        public abstract void Run(Node owner);
    }

    private class Exit : Command
    {
        public override void Run(Node owner){
            owner.GetTree().Quit();
        }
    }
}