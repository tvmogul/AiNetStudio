using System.Runtime.InteropServices;

public class PathManager
{
    public string? LastResolvedPath { get; private set; }

    public PathManager() { }

    public string GetWritableFolder(string folderName)
    {
        string basePath;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            basePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "Library",
                "Application Support");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            basePath = "/var";
        }
        else
        {
            basePath = Directory.GetCurrentDirectory();
        }

        string fullPath = Path.Combine(basePath, "AiNetStudio", folderName);

        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);

        LastResolvedPath = fullPath;
        return fullPath;
    }

    public string GetInstallationFolder(string folderName)
    {
        string basePath = AppContext.BaseDirectory;

        string fullPath = string.IsNullOrWhiteSpace(folderName)
            ? basePath
            : Path.Combine(basePath, folderName);

        LastResolvedPath = fullPath;
        return fullPath;
    }

    public string GetProgramFiles32(string folderName)
    {
        string basePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

        string fullPath = string.IsNullOrWhiteSpace(folderName)
            ? basePath
            : Path.Combine(basePath, folderName);

        LastResolvedPath = fullPath;
        return fullPath;
    }

    public string GetProgramFiles64(string folderName)
    {
        string basePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        string fullPath = string.IsNullOrWhiteSpace(folderName)
            ? basePath
            : Path.Combine(basePath, folderName);

        LastResolvedPath = fullPath;
        return fullPath;
    }
}
