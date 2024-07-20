namespace DirectoryTree
{
    public class Folder(string name)
{
    public string Name { get; set; } = name;
    private Dictionary<string, Folder> SubFolders { get; set; } = [];

    public Folder Add(string name)
    {
        var subFolder = new Folder(name);
        SubFolders.TryAdd(name, subFolder);
        return subFolder;
    }

    public Folder Add(Folder subFolder)
    {
        SubFolders.TryAdd(subFolder.Name, subFolder);
        return subFolder;
    }

    public bool Remove(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException($"{nameof(name)} cannot be null or empty");
        }

        return SubFolders.Remove(name);
    }

    private void List(int identation)
    {
        Console.WriteLine($"{new string(' ', identation)}{Name}");
        foreach (var folder in SubFolders.Values)
        {
            folder.List(identation + 2);
        }
    }

    /// <summary>
    /// List children of 'root'
    /// </summary>
    public void List() => SubFolders.Values.ToList().ForEach(f => f.List(0));

    public Folder? GetFolder(string name)
    {
        if (SubFolders.TryGetValue(name, out var folder))
        {
            return folder;
        }
        return null;
    }

}

}