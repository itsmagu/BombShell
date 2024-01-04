using System;

namespace BombShell.EmuSystemScope;

public class Processor
{
    
}

public struct Process(Action proc, User user, Action? init = null, Action? finalize = null)
{
    public Action? Init { get; } = init;
    public Action Proc { get; } = proc;
    public Action? Finalize { get; } = finalize;
    public User User { get; } = user;
    
}