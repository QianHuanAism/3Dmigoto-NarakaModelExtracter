using System.IO;
using System.Windows.Media;

namespace NMC.Utils.FMT;

public class FmtWriter
{
    private string writePath;
    private string ibHash;

    public FmtWriter(string writePath, string ibHash)
    {
        this.writePath = writePath;
        this.ibHash = ibHash;
    }

    public void Write(List<string> fmtContentList)
    {
        try
        {
            string filePath = Path.Combine(writePath, $"{ibHash}.fmt");

            if (!File.Exists(filePath))
            {
                using (File.Create(filePath)) { }
            }

            File.WriteAllLines(filePath, fmtContentList);
        }
        catch
        {

        }
    }
}
