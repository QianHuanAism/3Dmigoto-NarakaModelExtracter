using NMC.Helpers;
using System.IO;

namespace NMC.Utils.VB;

public class VBStrideCollector
{
    private StreamHelper streamBuilder = new StreamHelper();

    public Dictionary<string, string> GetStride(
        string frameAnalysis,
        Dictionary<string, List<string>> vbFiles
    )
    {
        Log.Info("收集 VB 文件的数据步长");
        Dictionary<string, string> strides = new Dictionary<string, string>();
        foreach (var ibHash in vbFiles.Keys)
        {
            foreach (var vbFile in vbFiles[ibHash])
            {
                using (var fs = streamBuilder.GetFileStream(Path.Combine(frameAnalysis, vbFile)))
                {
                    StreamReader sr = new StreamReader(fs);
                    string stride = sr.ReadLine()!.Split(": ")[1];
                    strides.Add(vbFile, stride);
                    Log.Info($"文件: \"{vbFile}\" --> 步长: {stride}");
                }
            }
        }

        return strides;
    }
}
