namespace BombShell.EmuSystemScope.Filesystem;

public class EmuFileSystem
{
    public bool Used { get; set; }
    public Folder RootContent { get; init; } = new Folder();
    public static Folder StandardFileSystem(){
        return new Folder() {
            Folders = {
                {
                    "bin", // Programs in lua
                    new Folder() {
                        Folders = { { "lua", new Folder() { Files = { "lua.lua" } } } }
                    }
                }, //
                {
                    "users", // all users on the system
                    new Folder() {
                        Files = {
                            "root",
                            "cool guy",
                            "nice guy",
                        }
                    }
                },
                "etc",
                "sys",
                "var",
                "dev",
                "boot",
                "libs",
                "mnt",
                "tmp"
            },
            Files = { "boot" }
        };
    }
};