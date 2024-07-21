namespace DirectoryTree;

/// <summary>
/// Holds the folder object with children
/// </summary>
/// <param name="name"></param>
public class Folder(string name)
{
    public string Name { get; set; } = name;
    private Dictionary<string, Folder> SubFolders { get; set; } = [];

    /// <summary>
    /// Adds a new child folder
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Folder Add(string name)
    {
        name = name.ToLower();
        var subFolder = new Folder(name);
        SubFolders.TryAdd(name, subFolder);
        return subFolder;
    }

    /// <summary>
    /// Adds an existing folder object to children folders
    /// </summary>
    /// <param name="subFolder"></param>
    /// <returns></returns>
    public Folder Add(Folder subFolder)
    {
        SubFolders.TryAdd(subFolder.Name.ToLower(), subFolder);
        return subFolder;
    }

    /// <summary>
    /// Removes a folder from subfolders
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool Remove(string name)
    {
        name = name.ToLower();
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException($"{nameof(name)} cannot be null or empty");
        }

        return SubFolders.Remove(name);
    }

    /// <summary>
    /// Prints current folder name + children
    /// </summary>
    /// <param name="identation"></param>
    private void List(int identation)
    {
        Console.WriteLine($"{new string(' ', identation)}{Name}");
        foreach (var folder in SubFolders.Values.OrderBy(f=> f.Name))
        {
            folder.List(identation + 2);
        }
    }

    /// <summary>
    /// List children of 'root'
    /// </summary>
    public void List() => SubFolders.Values.OrderBy(f=> f.Name).ToList().ForEach(ch => ch.List(0));

    public Folder? GetFolder(string name)
    {
        if (SubFolders.TryGetValue(name, out var folder))
        {
            return folder;
        }
        return null;
    }

}

