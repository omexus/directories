Console.WriteLine("Welcome to the Directory Tree!");
Console.WriteLine("Enter 'x' to quit");
string? command;


while ((command = Console.ReadLine()) != "x")
{
    CommandService.ExecuteCommand(command);
}

Console.WriteLine("Goodbye!");

/// <summary>
/// Service to handle commands
/// </summary>
public class CommandService
{
    /// <summary>
    /// Dictionary to hold the commands and their descriptions + correct syntax
    /// </summary>
    readonly static Dictionary<string, (string Description, int ParamNum, string syntax)> commands = new()
    {
        ["CREATE"] = ("Create a new folder", 1, "CREATE <new-folder>"),
        ["DELETE"] = ("Remove a Folder", 1, "DELETE <existing-folder>"),
        ["MOVE"] = ("Move to a folder", 2, "MOVE <existing-sourcefolder> <existing-targetfolder>"),
        ["LIST"] = ("List all folders", 0, "LIST")
    };

    /// <summary>
    /// Makes sure the command and its parameters are valid
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    private static bool ValidateCommand(string? command)
    {
        if (string.IsNullOrEmpty(command))
        {
            Console.WriteLine("Command cannot be empty");
            return false;
        }

        //split the command into parts
        var parts = command.Split(" ");
        //get the command (first in the list)
        var actionCommand = parts[0].ToUpper();

        //check if the command is valid and has the right number of parameters
        if (commands.TryGetValue(actionCommand, out (string Description, int ParamNum, string syntax) value))
        {
            if (value.ParamNum != parts.Length - 1)
            {
                Console.WriteLine($"Invalid number of parameters for {actionCommand}");
                Console.WriteLine($"Syntax: {value.syntax}");
                return false;
            }
            return true;
        }
        else
        {
            Console.WriteLine("Invalid Command: " + actionCommand);
            Console.WriteLine("Valid Commands are:");
            foreach (var item in commands)
            {
                Console.WriteLine($"{item.Key} - {item.Value.Description} - {item.Value.syntax}");
            }
            return false;
        }
    }

    /// <summary>
    /// Execute the command provided the command is valid
    /// </summary>
    /// <param name="command">e.g. CREATE some-folder</param>
    public static void ExecuteCommand(string? command)
    {
        command = command?.Trim();
        if (!ValidateCommand(command))
        {
            return;
        }

        var parts = command!.Split(" ");
        var actionCommand = parts[0].ToUpper();

        switch (actionCommand)
        {
            case "CREATE":
                FolderService.CreateFolder(parts[1]);
                break;
            case "MOVE":
                FolderService.MoveFolder(parts[1], parts[2]);
                break;
            case "DELETE":
                FolderService.DeleteFolder(parts[1]);
                break;
            case "LIST":
                FolderService.ListFolders();
                break;
            default:
                break;
        }
    }

}

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
        var (sourceFolder, message) = FindFolder(fullPath);
        if (sourceFolder == null)
        {
            Console.WriteLine(message);
            return null;
        }

        //remove the folder from the source
        DeleteFolder(fullPath);

        var (targetFolder, message2) = FindFolder(pathTo);
        if (targetFolder == null)
        {
            Console.WriteLine(message2);
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
        var parentPath = "";
        var message = "";

        //path maybe just on the root
        if (parts.Length == 1)
        {
            parentFolder = _root;
            message = $"{fullPath} dpes not exist";
        }
        else    //find parent folder
        {
            //get the parent folder path only
            parentPath = string.Join("/", parts[..^1]);
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

    private static (Folder? folder, string message) FindParentFolder(string path){
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
                return (null, message);
            }
        }      
        return (parent, "");
    }

    private static (Folder? folder, string message) FindFolder(string path)
    {
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
