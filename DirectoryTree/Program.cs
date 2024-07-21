using DirectoryTree;

Console.WriteLine("Welcome to the Directory Tree!");
Console.WriteLine("Enter 'x' to quit");
string? command;

while ((command = Console.ReadLine()) != "x")
{
    CommandService.ExecuteCommand(command);
}

Console.WriteLine("Goodbye!");