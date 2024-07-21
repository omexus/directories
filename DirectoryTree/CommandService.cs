namespace DirectoryTree
{
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
            Console.WriteLine("'x' + <enter> to quit");
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
}