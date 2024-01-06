using System.Collections.Generic;
using System.Linq;

namespace BombShell.EmuSystemScope.Filesystem;

public class Folder()
{
    // Folders in this folder
    public class FolderDirectories : SortedDictionary<string, Folder>
    {
        public void Add(string name){
            name = name.Replace(" ", "");
            if (ContainsKey(name)) return;
            base.Add(name, new Folder());
        }
    }

    public FolderDirectories Folders { get; } = [];

    // Files in this folder
    public class FolderContent : SortedDictionary<string, File>
    {
        public void Add(string name, string content){
            name = name.Replace(" ", "");
            if (ContainsKey(name)) return;
            base.Add(name, new File(content));
        }
        public void Add(string name){
            name = name.Replace(" ", "");
            if (ContainsKey(name)) return;
            base.Add(name, new File(""));
        }
    }

    public FolderContent Files { get; } = [];

    // Methods
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
            foreach (KeyValuePair<string, Folder> subDirectory in folder.Folders){
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