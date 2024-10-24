namespace Server.Library;

public class FileManager
{
    public static string AppDataDir { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public string CurrentDirectory { get; private set; }

    private FileManager(string currentDirectory)
        => this.CurrentDirectory = currentDirectory;

    public static FileManager Create(string directory)
        => new(directory);

    private string[] AddCurrentDirectory(string[] parameters)
    {
        List<string> values = new() { this.CurrentDirectory };
        values.AddRange(parameters);
        return values.ToArray();
    }

    public static string Concat(params string[] parameters)
        => string.Join(Path.DirectorySeparatorChar, parameters);

    public string PathJoin(params string[] parameters)
        => Concat(this.AddCurrentDirectory(parameters));
    
    public List<string> LsDir()
        => Directory.GetDirectories(this.CurrentDirectory).ToList();

    public bool ExistsDirectory(string directory)
        => Directory.Exists(this.PathJoin(directory));    

    public bool CurrentDirExists()
        => ExistsDirectory(this.CurrentDirectory);

    public string Mkdir(string directory)
    {
        var path = this.PathJoin(directory);
        if (this.ExistsDirectory(path))
            throw new ArgumentException($"{directory} exists");

        Directory.CreateDirectory(path);
        return path;
    }
    
    public FileManager NextDirectory(string nextDirectory)
    {
        if (!this.ExistsDirectory(nextDirectory))
            throw new ArgumentException($"{nextDirectory} not exists in {this.CurrentDirectory}");

        return new(this.PathJoin(nextDirectory));
    }
    
    public List<string> LsFiles()
        => Directory.GetFiles(this.CurrentDirectory).ToList();

    public bool FileExists(string fileName)
        => File.Exists(this.PathJoin(fileName));
    
    public bool FileExists()
        => File.Exists(this.CurrentDirectory);

    public string RmFile(string fileName)
    {
        var path = this.PathJoin(fileName);
        
        if (this.FileExists(fileName))
            File.Delete(path);

        return path;
    }
    
    public string RmFile()
    {
        if (this.FileExists())
            File.Delete(this.CurrentDirectory);

        return this.CurrentDirectory;
    }
    
    public string WriteFile(string fileName, string content)
    {
        var path = this.RmFile(fileName);
        File.WriteAllText(path, content);
        return path;
    }
    
    public string WriteFile(string fileName, byte[] content)
    {
        var path = this.RmFile(fileName);
        File.WriteAllBytes(path, content);
        return path;
    }
    
    public string? ReadFile(string fileName)
    {
        var path = this.PathJoin(fileName);
        return (this.FileExists(fileName)) ? File.ReadAllText(path) : null;
    }
    
    public byte[] ReadFile()
    {
        if (!this.FileExists())
            return Array.Empty<byte>();

        return File.ReadAllBytes(this.CurrentDirectory);
    }
}