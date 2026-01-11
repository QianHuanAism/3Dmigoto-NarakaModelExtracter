using NMC.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NMC.Helpers;

public class VBStrideCollector(string frameAnalysis, Dictionary<string, List<string>> vbFiles)
{
    private StreamBuilder streamBuilder = new StreamBuilder();

    /// <summary>
    /// 获取VB文件中stride字段的值
    /// </summary>
    /// <returns>由文件名为Key, stride字段的值为Value的字典</returns>
    public Dictionary<string, string> GetStride()
    {
        Dictionary<string, string> strides = new Dictionary<string, string>();

        foreach (var ibHash in vbFiles.Keys)
        {
            foreach (var vbFile in vbFiles[ibHash])
            {
                using (var fs = streamBuilder.GetFileStream(Path.Combine(frameAnalysis, vbFile)))
                {
                    StreamReader sr = new StreamReader(fs);
                    strides.Add(vbFile, sr.ReadLine()!.Split(": ")[1]);
                }
            }
        }

        return strides;
    }
}
