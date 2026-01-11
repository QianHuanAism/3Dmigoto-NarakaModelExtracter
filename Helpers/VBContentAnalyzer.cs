using Dumpify;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace NMC.Helpers;

public record VBContentAnalyzer(string frameAnalysis, Dictionary<string, List<string>> vbFiles)
{
    private StreamBuilder streamBuilder = new StreamBuilder();

    public void SemanticBlockExtractor(List<Dictionary<string, Dictionary<string, string>>> semanticNameList)
    {
        Dictionary<string, List<string>>? fileSemanticBlockMap = new Dictionary<string, List<string>>();
        foreach (var fileSemanticNameMap in semanticNameList)
        {
            foreach (var file in fileSemanticNameMap.Keys)
            {
                string vbFile = Path.Combine(frameAnalysis, file);
                List<string> contentList = new List<string>();
                List<string> semanticBlockList = new List<string>();

                using var fs = streamBuilder.GetFileStream(vbFile);
                StreamReader sr = new StreamReader(fs);
                string? line;
                while ((line = sr.ReadLine()) is not "vertex-data:")
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    contentList.Add(line.TrimStart());
                }

                foreach (var semanticName in fileSemanticNameMap[file].Keys)
                {
                    for (int i = 0; i < contentList.Count; i++)
                    {
                        if (contentList[i].Replace(" ", "").Equals($"SemanticName:{semanticName}"))
                        {
                            int offset = i + 6;
                            for (int j = i; j <= offset; j++)
                            {
                                semanticBlockList.Add(contentList[j]);
                            }
                        }
                    }
                }
                semanticBlockList.Dump();
            }
        }
    }
}
