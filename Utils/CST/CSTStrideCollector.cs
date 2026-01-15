using NMC.Helpers;
using System.IO;

namespace NMC.Utils.CST;

public class CSTStrideCollector
{
    private StreamHelper streamBuilder;
    private string frameAnalysisPath;

    public CSTStrideCollector(string frameAnalysisPath)
    {
        this.frameAnalysisPath = frameAnalysisPath;
        streamBuilder = new StreamHelper();
    }

    public Dictionary<string, string> GetCSTStride(
        Dictionary<string, string> vertexCounts,
        List<string> cstFileList
    )
    {
        Dictionary<string, string> cstStrideMap = new Dictionary<string, string>();
        foreach (var cstFile in cstFileList)
        {
            foreach (var vbFile in vertexCounts.Keys)
            {
                string vertexCount = vertexCounts[vbFile];
                using var fs = streamBuilder.GetFileStream(
                    Path.Combine(frameAnalysisPath, cstFile)
                );
                long fileSize = fs.Length;
                string cstStride = (fileSize / int.Parse(vertexCount)).ToString();
                if (!cstStrideMap.ContainsKey(cstFile))
                    cstStrideMap.Add(cstFile, cstStride);
            }
        }

        // 其实应该还有一层判断, 但是感觉没必要, 因为VB的顶点数大概率不会变化, 不然的话步长就有问题了, 感觉没必要再做一层如果顶点数不一样导致字典没元素的判断
        return cstStrideMap;
    }
}
