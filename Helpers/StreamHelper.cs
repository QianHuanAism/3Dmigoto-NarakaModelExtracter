using System.IO;

namespace NMC.Helpers;

public record StreamHelper(
    FileMode fileMode = FileMode.Open,
    FileAccess fileAccess = FileAccess.ReadWrite
)
{
    public FileStream GetFileStream(string filePath)
    {
        return new FileStream(filePath, fileMode, fileAccess);
    }
}
