using System.Collections.Generic;

namespace BombShell.ShellScope;

public interface IShell
{
    void PrintToShell(string message);
    Queue<string> CommandQueue { get; }
}