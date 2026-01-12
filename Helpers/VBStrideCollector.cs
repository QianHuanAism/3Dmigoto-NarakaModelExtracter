using NMC.Model;
using NMC.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NMC.Helpers;

public class VBStrideCollector
{
    private StreamBuilder streamBuilder = new StreamBuilder();

    /// <summary>
    /// 获取VB文件中stride字段的值
    /// </summary>
    /// <returns>由文件名为Key, stride字段的值为Value的字典</returns>
    public Dictionary<string, string> GetStride(string frameAnalysis, Dictionary<string, List<string>> vbFiles)
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
