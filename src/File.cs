namespace BombShell.EmuSystemScope.Filesystem;

public partial class File(string content) : Godot.Resource
{
    public string Content { get; } = content;
}