using NMC.Helpers;
using System.IO;

namespace NMC.Utils.VB;

public class VBCollector
{
    public Dictionary<string, List<string>>? CollectTxtVBFile(
        string frameAnalysis,
        Dictionary<string, List<string>> ibDrawCallMap
    )
    {
        Dictionary<string, List<string>> vbFiles = new Dictionary<string, List<string>>();
        foreach (var ibHash in ibDrawCallMap.Keys)
        {
            List<string> drawCalls = ibDrawCallMap[ibHash];
            List<string> vbFileList = new List<string>();
            foreach (var drawCall in drawCalls)
            {
                foreach (var file in Directory.GetFiles(frameAnalysis).ToList())
                {
                    if (file.Contains($"{drawCall}-vb") && Path.GetExtension(file).Equals(".txt"))
                    {
                        vbFileList.Add(Path.GetFileName(file));
                        Log.Info($"绘制调用: {drawCall} --> {Path.GetFileName(file)}");
                    }
                }

                if (vbFileList.Count <= 0)
                {
                    Log.Err(
                        $"未找到绘制调用 {drawCall} 对应的 VB 文件提取失败, 开始提取下一个 DrawIB"
                    );
                    return null;
                }
                if (!vbFiles.ContainsKey(ibHash))
                    vbFiles.Add(ibHash, vbFileList);
            }
        }

        return vbFiles;
    }

    public Dictionary<string, List<string>>? CollectBufVBFile(
        string frameAnalysis,
        Dictionary<string, List<string>> ibDrawCallMap
    )
    {
        Dictionary<string, List<string>> vbFiles = new Dictionary<string, List<string>>();
        foreach (var ibHash in ibDrawCallMap.Keys)
        {
            List<string> drawCalls = ibDrawCallMap[ibHash];
            List<string> vbFileList = new List<string>();
            foreach (var drawCall in drawCalls)
            {
                foreach (var file in Directory.GetFiles(frameAnalysis).ToList())
                {
                    if (file.Contains($"{drawCall}-vb") && Path.GetExtension(file).Equals(".buf"))
                    {
                        vbFileList.Add(Path.GetFileName(file));
                        Log.Info($"绘制调用: {drawCall} --> {Path.GetFileName(file)}");
                    }
                }

                if (vbFileList.Count <= 0)
                {
                    Log.Err(
                        $"未找到绘制调用 {drawCall} 对应的 VB 文件提取失败, 开始提取下一个 DrawIB"
                    );
                    return null;
                }
                if (!vbFiles.ContainsKey(ibHash))
                    vbFiles.Add(ibHash, vbFileList);
            }
        }
        return vbFiles;
    }
}
