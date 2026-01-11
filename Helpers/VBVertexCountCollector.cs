using NMC.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NMC.Helpers;

public class VBVertexCountCollector(string frameAnalysis, Dictionary<string, List<string>> vbFiles)
{
    private StreamBuilder streamBuilder = new StreamBuilder();

    /// <summary>
    /// 获取VB文件中vertex count字段的值
    /// </summary>
    public Dictionary<string, string> GetVBVertexCount()
    {
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
                            vertexCounts.Add(vbFile, line.Split(": ")[1]);
                            break;
                        }
                    }
                }
            }
        }

        return vertexCounts;
    }
}
