using System.IO;

namespace NMC.Helpers;

public class PathHelper
{
    private string writePath;
    private string ibHash;

    public PathHelper(string writePath, string ibHash)
    {
        this.writePath = writePath;
        this.ibHash = ibHash;
    }

    public string GetWritePath()
    {
        string dirPath = Path.Combine(writePath, ibHash);
        CreateLackingDirecotory(dirPath);

        return dirPath;
    }

    private void CreateLackingDirecotory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }
}
