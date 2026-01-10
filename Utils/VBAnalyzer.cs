using Dumpify;
using NMC.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace NMC.Utils;

public record VBAnalyzer(
    string frameAnalysis, Dictionary<string, List<string>> vbFileDict, ObservableCollection<DrawIB> drawIBList)
{
    public void VBFileAnalysis()
    {
        foreach (var drawIB in drawIBList)
        {
            List<string> vbFiles = vbFileDict[drawIB.IBHash];
            Dictionary<string, Dictionary<string, List<string>>> resultDict = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (var file in vbFiles)
            {
                Dictionary<string, List<string>> strideInputElementBlock = new Dictionary<string, List<string>>();
                if (!Path.GetExtension(file).Equals(".txt"))
                    continue;

                string vbFile = Path.Combine(frameAnalysis, file);
                Dictionary<string, string> semanticNameDict = GetCorrectSemantics(vbFile);
                semanticNameDict.Dump();
                string stride = GetVBFileStride(vbFile);
                string vertexCount = GetVertexCount(vbFile);
                List<string> inputElementBlock = GetCorrectInputElementBlock(vbFile, semanticNameDict);

                if (!strideInputElementBlock.ContainsKey(stride))
                    strideInputElementBlock.Add(stride, inputElementBlock);

                if (!strideInputElementBlock.ContainsKey(file))
                    resultDict.Add(file, strideInputElementBlock);

                SemanticBlockAnalyzer(resultDict);
            }
        }
    }

    private Dictionary<string, string> GetCorrectSemantics(string vbFile)
    {
        using (FileStream fs = new FileStream(vbFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            StreamReader sr = new StreamReader(fs);
            Dictionary<string, string> vertexDataDict = new Dictionary<string, string>();
            List<string> vbContentList = new List<string>();
            Dictionary<string, string> semanticNameDict = new Dictionary<string, string>();
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                vbContentList.Add(line);
            }

            for (int index = vbContentList.IndexOf("vertex-data:") + 1; index < vbContentList.Count; index++)
            {
                string semanticName = vbContentList[index].Split(": ")[0].Split(" ")[1];
                string semanticValue = vbContentList[index].Split(": ")[1];
                if (!vertexDataDict.ContainsKey(semanticName) && !vertexDataDict.ContainsValue(semanticValue))
                {
                    vertexDataDict.Add(semanticName, semanticValue);
                }
                else
                {
                    break;
                }
            }

            foreach (var semanticName in vertexDataDict.Keys)
            {
                string[] semantics = semanticName.Substring(0).Split("D");
                if (semantics.Length < 2 || string.IsNullOrEmpty(semantics[1]))
                {
                    semanticNameDict.Add(semanticName, "0");
                }
                else
                {
                    semanticNameDict.Add(semanticName, semantics[1]);
                }
            }

            return semanticNameDict;
        }
    }

    private string GetVBFileStride(string vbFile)
    {
        using (FileStream fs = new FileStream(vbFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            StreamReader sr = new StreamReader(fs);
            string line = sr.ReadLine()!;

            return line.Split(": ")[1];
        }
    }

    private string GetVertexCount(string vbFile)
    {
        using (FileStream fs = new FileStream(vbFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            StreamReader sr = new StreamReader(fs);
            string line = "";
            for (int i = 0; i < 3; i++)
            {
                line = sr.ReadLine()!;
            }

            return line.Split(": ")[1];
        }
    }

    private List<string> GetCorrectInputElementBlock(string vbFile, Dictionary<string, string> semanticNameDict)
    {
        Dictionary<int, Dictionary<string, string>> elementDict = new Dictionary<int, Dictionary<string, string>>();
        List<string> vbContentList = new List<string>();
        List<string> inputElementBlock = new List<string>();
        using (FileStream fs = new FileStream(vbFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            StreamReader sr = new StreamReader(fs);
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                vbContentList.Add(line);
            }

            int endIndex = vbContentList.IndexOf("vertex-data:");

            for (int i = 0; i < endIndex - 1; i++)
            {
                foreach (var semanticName in semanticNameDict.Keys)
                {
                    if (vbContentList[i].Contains($"SemanticName: {semanticName}")
                        && vbContentList[i + 1].Contains($"SemanticIndex: {semanticNameDict[semanticName]}"))
                    {
                        for (int j = i; j <= i + 6; j++)
                        {
                            inputElementBlock.Add(vbContentList[j].TrimStart());
                        }
                    }
                }
            }

            return inputElementBlock;
        }
    }

    private void SemanticBlockAnalyzer(Dictionary<string, Dictionary<string, List<string>>> analyzeSource)
    {
        foreach (var file in analyzeSource.Keys)
        {
            var inputElementBlock = analyzeSource[file];
            foreach (var stride in analyzeSource[file].Keys)
            {
                inputElementBlock[stride].Dump();
            }
        }
    }
}
