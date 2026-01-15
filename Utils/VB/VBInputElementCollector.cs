using NMC.Helpers;
using System.IO;

namespace NMC.Utils.VB;

public class VBInputElementCollector
{
    private StreamHelper streamBuilder = new StreamHelper();

    public List<Dictionary<string, List<string>>> GetVBInputElementList(
        string frameAnalysis,
        Dictionary<string, List<string>> vbFiles,
        List<Dictionary<string, Dictionary<string, string>>> semanticList
    )
    {
        List<Dictionary<string, List<string>>> fileSemanticBlockList =
            new List<Dictionary<string, List<string>>>();
        foreach (var fileSemanticMap in semanticList)
        {
            Dictionary<string, List<string>> fileSemanticBlockMap =
                new Dictionary<string, List<string>>();
            foreach (var file in fileSemanticMap.Keys)
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

                foreach (var semanticName in fileSemanticMap[file].Keys)
                {
                    for (int i = 0; i < contentList.Count; i++)
                    {
                        if (
                            contentList[i].Equals($"SemanticName: {semanticName}")
                            && contentList[i + 1]
                                .Equals($"SemanticIndex: {fileSemanticMap[file][semanticName]}")
                        )
                        {
                            int offset = i + 6;
                            for (int j = i; j <= offset; j++)
                            {
                                semanticBlockList.Add(contentList[j]);
                            }
                        }
                    }
                }

                if (semanticBlockList.Count < 1)
                {
                    continue;
                }

                fileSemanticBlockMap.Add(file, semanticBlockList);
            }
            fileSemanticBlockList.Add(fileSemanticBlockMap);
        }

        return fileSemanticBlockList;
    }
}
