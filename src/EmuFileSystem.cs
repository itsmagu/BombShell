using System.Collections.Generic;
using System.Linq;

namespace BombShell;

public class EmuFileSystem
{
    public bool Used { get; set; } = false;
    public Folder Root { get; } = new Folder();
};

public class Folder()
{
    public Dictionary<string, Folder> SubDirectories { get; } =
        new Dictionary<string, Folder>();
    public Dictionary<string, File> Files { get; } = new Dictionary<string, File>();
    public string ListAllRecursive(int maxDepth = 1){
        string s = "";
        int depth = 0;
        Recurse(this);
        return s;
        void Recurse(Folder folder){
            if (depth == maxDepth) return;
            depth++;
            string depthLevel = "";
            for (int i = 0; i < depth - 1; i++){
                depthLevel += " ";
            }
            foreach (KeyValuePair<string, Folder> subDirectory in folder.SubDirectories){
                s += $"\n{depthLevel}{subDirectory.Key}:";
                Recurse(subDirectory.Value);
            }
            s = folder.Files.Keys.Aggregate(
                s,
                (current, filesKey) => current + $"\n{depthLevel}{filesKey}"
            );
            depth--;
        }
    }
}

public class File(string content)
{
    public string Content { get; } = content;
}