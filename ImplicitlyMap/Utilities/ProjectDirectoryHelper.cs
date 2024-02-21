namespace ImplicitlyMap.Utilities;

public static class ProjectDirectoryHelper
{
    public static string FindDirectoryByName(string folderName)
    {
        var rootDirectory = FindProjectRootDirectory();
        if (string.IsNullOrWhiteSpace(rootDirectory))
            throw new DirectoryNotFoundException("Proje kök dizini bulunamadı.");

        var directories = Directory.GetDirectories(rootDirectory, folderName, SearchOption.AllDirectories);
        if (directories.Length == 0)
            throw new DirectoryNotFoundException($"{folderName} adında bir klasör bulunamadı.");

        return directories[0];
    }

    private static string FindProjectRootDirectory()
    {
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (currentDirectory is not null)
        {
            if (currentDirectory.Name.Equals("bin", StringComparison.OrdinalIgnoreCase) ||
                currentDirectory.Name.Equals("obj", StringComparison.OrdinalIgnoreCase))
            {
                return currentDirectory.Parent?.Parent?.FullName ??
                       throw new InvalidOperationException("Proje kök dizini bulunamadı.");
            }

            currentDirectory = currentDirectory.Parent;
        }

        throw new InvalidOperationException("Proje kök dizini bulunamadı.");
    }
}