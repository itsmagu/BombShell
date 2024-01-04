using System.Collections.Generic;
using System.Linq;

namespace BombShell.EmuSystemScope;

public class EmuFileSystem
{
    public bool Used { get; set; }
    public Folder RootContent { get; init; } = new Folder();
    public static Folder StandardFileSystem(){
        return new Folder() {
            Folders = {
                "bin", {
                    "home", new Folder() {
                        Files = {
                            "root",
                            "coolcool",
                            "niceguy"
                        }
                    }
                },
                "etc",
                "root",
                "boot"
            },
            Files = {
                "boot",
                "user",
                "boot"
            }
        };
    }
};

public class Folder()
{
    public class FolderDirectories : SortedDictionary<string, Folder>
    {
        public void Add(string name){
            if (ContainsKey(name)) return;
            base.Add(name, new Folder());
        }
    }

    public class FolderContent : SortedDictionary<string, File>
    {
        public void Add(string name, string content){
            if (ContainsKey(name)) return;
            base.Add(name, new File(content));
        }
        public void Add(string name){
            if (ContainsKey(name)) return;
            base.Add(name, new File(""));
        }
    }

    public FolderDirectories Folders { get; } = [];
    public FolderContent Files { get; } = [];
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

public class File(string content)
{
    public string Content { get; } = content;
}