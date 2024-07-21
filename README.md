
# Directories

Directories is a C# CLI program to create 'virtual' directories, i.e. it supports commands that allow a user to create, move and delete directories.

## Installation / Run

clone this repository

Go to 'Packages' folder and choose the zip file depending of your system (osx-arm64.zip). Unzip it, this will give you an executable named:
```bash
DirectoryTree
```

## Sample Run

```
Welcome to the Directory Tree!
Enter 'x' to quit

CREATE fruits
CREATE vegetables
CREATE grains
CREATE fruits/apples
CREATE fruits/apples/fuji
LIST

```

## In case none  of the packages is suitable...
1. Install/Open vscode
2. Make sure you have the basics for Dev Container mode development (https://code.visualstudio.com/docs/devcontainers/tutorial) - basically have Docker & 'Dev Container' extension installed
3. Open Folder (e.g. 'directories')

4. From the terminal, go to the 'cloned' folder (e.g. 'directories')
```bash
cd DirectoryTree
dotnet run
```

## License
Unlicensed