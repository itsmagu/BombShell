namespace BombShell;

public class FileSystem
{
    public interface IFileSystemPoint
    {
        void Open();
        string Name();
    }

    public struct File(string name) : IFileSystemPoint
    {
        public void Open(){ }
        public string Name() => name;
    }

    public struct Folder(string name, IFileSystemPoint[] content) : IFileSystemPoint
    {
        public IFileSystemPoint[] Content = content;
        public void Open(){ }
        public string Name() => name;
    }
}