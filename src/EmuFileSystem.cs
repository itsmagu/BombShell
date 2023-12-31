using System.Collections.Generic;
using System.Linq;

namespace BombShell;

public class EmuFileSystem
{
    public string Name { get; } = "Nameless";
    public static List<User> Users = [new User("root", true)];
    public Directory Root { get; } = new Directory(
        "root",
        [
            new File("boot", "boot file data"),
        ]
    );
}

public interface IFsPoint
{
    public string Name { get; }
}

public struct Directory(string name, List<IFsPoint> content) : IFsPoint
{
    public string Name { get; } = name;
    public List<IFsPoint> Content { get; } = content;
    public override string ToString(){
        string s = Content.Aggregate("<", (current, fsPoint) => current + $" {fsPoint.Name}");
        return s + " >";
    }
    public PermissionsFlags Perms { get; } = PermissionsFlags.Run | PermissionsFlags.Open |
        PermissionsFlags.Edit | PermissionsFlags.Move;
}

public struct File(string name, string content) : IFsPoint
{
    public string Name { get; } = name;
    public string Content { get; } = content;
    public override string ToString() => Content;
}