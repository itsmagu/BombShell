using System;

namespace BombShell;

public class Processor
{
    
}

public struct Process(Action proc, User user)
{
    public Action Proc { get; } = proc;
    public User User { get; } = user;
}