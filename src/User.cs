using System;

namespace BombShell.EmuSystemScope.Software;

public struct User(string name, bool rootAccess)
{
    public string Name { get; } = name;
    public bool RootAccess { get; } = rootAccess;
}

[Flags]
public enum PermissionsFlags : ushort
{
    Run = 1,
    Open = 2,
    Edit = 4,
    Move = 8
}