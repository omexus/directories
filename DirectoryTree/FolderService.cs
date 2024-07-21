namespace DirectoryTree;

/// <summary>
/// Service to handle folder operations
/// </summary>
public class FolderService
{
    //root folder (invisible) holds the reference to everything underneath
    private static readonly Folder _root = new("/");

    /// <summary>
    /// Create a folder in the directory tree using a starting parent folder (root)
    /// </summary>
    /// <param name="path">string representation of the folder to add</param>
    /// <returns>Newly created folder</returns>
    public static Folder? CreateFolder(string path)
    {
        //break the path into parts
        var parts = path.Split("/");
        //get the name of the folder (last part of the path)
        var name = parts[^1];
        //get the parent folder path (by rejoing all parts except the last one)
        var parentPath = string.Join("/", parts[..^1]);

        //find parent folder
        Folder? parent;

        if (parentPath == "")
        {
            parent = _root; //root folder
        }
        else
        {
            //find parent folder
            (parent, var message) = FindFolder(parentPath);

            if (parent == null)
            {
                Console.WriteLine(message);
                return null;
            }
        }

        //add the folder to the parent folder
        return parent.Add(name);
    }

    /// <summary>   
    /// Move a (leaf) folder from one location to another
    /// </summary>
    /// <param name="fullPath">source folder path</param>
    /// <param name="pathTo">target folder path</param>
    /// <returns>Moved folder</returns>
    public static Folder? MoveFolder(string fullPath, string pathTo)
    {
        //get the folder to move (we want the reference so that it brings the children along)
        var (sourceFolder, message) = FindFolder(fullPath);
        if (sourceFolder == null)
        {
            Console.WriteLine(message);
            return null;
        }

        //remove the folder from the source
        DeleteFolder(fullPath);

        var (targetFolder, toMessage) = FindFolder(pathTo);
        if (targetFolder == null)
        {
            Console.WriteLine(toMessage);
            return null;
        }

        return targetFolder.Add(sourceFolder);
    }

    /// <summary>
    /// Delete (leaf) folder given a full path 
    /// </summary>
    /// <param name="fullPath"></param>
    public static void DeleteFolder(string fullPath)
    {
        //get the folder to delete by examining the path and finding the parent folder
        var parts = fullPath.Split("/");

        Folder? parentFolder;
        string? message;

        //path maybe just on the root
        if (parts.Length == 1)
        {
            parentFolder = _root;
            message = $"{fullPath} does not exist"; //in case is needed
        }
        else    //find parent folder
        {
            //get the parent folder path only
            string? parentPath = string.Join("/", parts[..^1]);
            //find parent folder
            (parentFolder, message) = FindFolder(parentPath);
        }

        //remove folder from parent 
        if (parentFolder == null || !parentFolder.Remove(parts[^1]))
        {
            Console.WriteLine($"Cannot delete {fullPath} - {message}");
        }
    }

    public static void ListFolders()
    {
        _root.List();
    }

    private static (Folder? folder, string message) FindFolder(string path)
    {
        if (path == "/") return (_root, "");

        var folderParts = path.Split("/");

        if (folderParts.Length == 1)
        {
            var folder = _root.GetFolder(folderParts[0]);

            if (folder != null)
            {
                return (folder, "");
            }
            else
            {
                return (null, $"{folderParts[0]} does not exist");
            }
        }

        var folderPath = "";
        var msg = "";

        Folder? resultingFolder = _root;
        //find folder in tree
        foreach (var folderPart in folderParts)
        {
            folderPath += folderPath != "" ? "/" + folderPart : folderPart;
            var folder = resultingFolder.GetFolder(folderPart);
            if (folder != null)
            {
                resultingFolder = folder;
            }
            else
            {
                msg = $"{folderPath} does not exist";
                resultingFolder = null;
                break;
            }
        }

        return (resultingFolder, msg);
    }
}

