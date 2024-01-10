using System;
using BombShell.ShellScope;
using Laylua.Library;

namespace BombShell.EmuSystemScope.Software;

public class Process(Action<ulong,IShell?,Lua> proc, User user, Action? init = null, Action? finalize = null)
{
    public Action<ulong,IShell?,Lua> Proc { get; } = proc;
    public User User { get; } = user;
    public Action? Init { get; } = init;
    public Action? Finalize { get; } = finalize;
    public Lua LuaMachine { get; } = new Lua();
}