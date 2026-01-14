using NMC.Helpers;
using NMC.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NMC.Utils.VB;

public class VBVertexCountCollector
{
    private StreamHelper streamBuilder = new StreamHelper();

    /// <summary>
    /// 获取VB文件中vertex count字段的值
    /// </summary>
    public Dictionary<string, string> GetVBVertexCount(string frameAnalysis, Dictionary<string, List<string>> vbFiles)
    {
        Log.Info("收集 VB 文件的顶点数");
        Dictionary<string, string> vertexCounts = new Dictionary<string, string>();
        foreach (var ibHash in vbFiles.Keys)
        {
            foreach (var vbFile in vbFiles[ibHash])
            {
                using (var fs = streamBuilder.GetFileStream(Path.Combine(frameAnalysis, vbFile)))
                {
                    StreamReader sr = new StreamReader(fs);
                    string line;
                    while ((line = sr.ReadLine()!) is not null)
                    {
                        if (line.StartsWith("vertex count:"))
                        {
                            string vertexCount = line.Split(": ")[1];
                            vertexCounts.Add(vbFile, vertexCount);
                            Log.Info($"文件: \"{vbFile}\" --> 顶点数: {vertexCount}");
                            break;
                        }
                    }
                }
            }
        }

        return vertexCounts;
    }
}
