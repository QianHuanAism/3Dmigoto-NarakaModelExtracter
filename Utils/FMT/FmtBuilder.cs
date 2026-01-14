using Dumpify;
using NMC.Helpers;
using NMC.Model;
using System.Drawing;
using System.IO;
using System.Windows;

namespace NMC.Utils.FMT;

public enum FmtState
{

}

public class FmtBuilder
{
    private FmtFixer fmtFixer;
    private Dictionary<string, string> vbStrides;
    private Dictionary<string, string> cstStrides;
    private List<Dictionary<string, List<string>>> vbInputElementList;
    private List<Dictionary<string, List<string>>> cstInputElementList;
    private Dictionary<string, List<string>> vbBufFiles;
    private Dictionary<string, List<string>> ibTxtFiles;
    private Dictionary<string, List<string>> ibBufFiles;
    private string frameAnalysisPath;

    public FmtBuilder(
        Dictionary<string, string> vbStrides,
        Dictionary<string, string> cstStrides,
        List<Dictionary<string, List<string>>> vbInputElementList,
        List<Dictionary<string, List<string>>> cstInputElementList,
        Dictionary<string, List<string>> vbBufFiles,
        Dictionary<string, List<string>> ibTxtFiles,
        Dictionary<string, List<string>> ibBufFiles,
        string frameAnalysisPath
        )
    {
        this.vbStrides = vbStrides;
        this.cstStrides = cstStrides;
        this.vbInputElementList = vbInputElementList;
        this.cstInputElementList = cstInputElementList;
        this.vbBufFiles = vbBufFiles;
        this.ibTxtFiles = ibTxtFiles;
        this.ibBufFiles = ibBufFiles;
        this.frameAnalysisPath = frameAnalysisPath;
    }

    public List<string> Build()
    {
        // 修复输入元素
        FmtFixer fmtFixer = new FmtFixer(vbStrides, vbInputElementList, vbBufFiles);
        Dictionary<string, List<string>> fixedFileElementMap = fmtFixer.FixElement();
        vbInputElementList.Clear();
        vbInputElementList.Add(fixedFileElementMap);
        // 合并输入元素
        List<string> totalInputElementList = MergeInputElement(vbInputElementList, cstInputElementList);
        string stride = GetTotalStride(totalInputElementList);
        Dictionary<string, string> alignedByteOffsetMap = CalcAlignedByteOffset(totalInputElementList);
        totalInputElementList = RebuildAlignedByteOffset(totalInputElementList, alignedByteOffsetMap);
        totalInputElementList = AddElementIndex(totalInputElementList);
        totalInputElementList = AddSpace(totalInputElementList);
        totalInputElementList = [.. GetIBFormat(), .. totalInputElementList];
        totalInputElementList.Insert(0, $"stride: {stride}");

        return totalInputElementList;
    }

    private List<string> AddSpace(List<string> totalInputElementList)
    {
        List<string> tempList = new List<string>();
        tempList.AddRange(totalInputElementList);
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].Contains("element[") || tempList[i].Contains("SemanticName: "))
            {
                continue;
            }

            tempList[i] = $"  {tempList[i]}";
        }

        return tempList;
    }

    private List<string> MergeInputElement(List<Dictionary<string, List<string>>> vbInputElementList, List<Dictionary<string, List<string>>> cstInputElementList)
    {
        List<string> totalInputElementList = new List<string>();
        foreach (var fileElementMap in vbInputElementList)
        {
            foreach (var vbFile in fileElementMap.Keys)
            {
                totalInputElementList.AddRange(fileElementMap[vbFile]);
            }
        }

        foreach (var fileElementMap in cstInputElementList)
        {
            foreach (var cstFile in fileElementMap.Keys)
            {
                totalInputElementList.AddRange(fileElementMap[cstFile]);
            }
        }

        return totalInputElementList;
    }

    private Dictionary<string, string> CalcAlignedByteOffset(List<string> totalInputElementList)
    {
        int start = -1;
        int current = 0;
        int byteOffset = 0;
        int previousTotal = 0;
        Dictionary<string, string> alignedByteOffsetMap = new Dictionary<string, string>();
        for (int i = 0; i < totalInputElementList.Count; i++)
        {
            string inputElement = totalInputElementList[i];
            if (inputElement.StartsWith("SemanticName:"))
            {
                start++;
                if (start == 0)
                {
                    current = DXGIFormat.DXGIFormapMaps[totalInputElementList[i + 2].Split(": ")[1]];
                    byteOffset = byteOffset + previousTotal;
                    previousTotal = current;
                    alignedByteOffsetMap.Add(inputElement.Split(": ")[1], byteOffset.ToString());
                }
                else
                {
                    current = DXGIFormat.DXGIFormapMaps[totalInputElementList[i + 2].Split(": ")[1]];
                    byteOffset = byteOffset + previousTotal;
                    previousTotal = current;
                    alignedByteOffsetMap.Add(inputElement.Split(": ")[1], byteOffset.ToString());
                }
            }
        }
        return alignedByteOffsetMap;
    }

    private List<string> RebuildAlignedByteOffset(List<string> totalInputElementList, Dictionary<string, string> alignedByteOffsetList)
    {
        int index = 0;
        List<string> newTotalInputElementList = new List<string>();
        newTotalInputElementList.AddRange(totalInputElementList);
        foreach (var inputElement in totalInputElementList)
        {
            foreach (var elementName in alignedByteOffsetList.Keys)
            {
                if (inputElement.StartsWith($"SemanticName: {elementName}"))
                {
                    newTotalInputElementList[index + 4] = $"AlignedByteOffset: {alignedByteOffsetList[elementName]}";
                }
            }
            index++;
        }

        return newTotalInputElementList;
    }

    private string GetTotalStride(List<string> totalInputElementList)
    {
        int totalStride = 0;
        foreach (var inputElement in totalInputElementList)
        {
            if (inputElement.StartsWith("Format: "))
            {
                totalStride += DXGIFormat.DXGIFormapMaps[inputElement.Split(": ")[1]];
            }
        }

        return totalStride.ToString();
    }

    private List<string> AddElementIndex(List<string> totalInputElementList)
    {
        List<string> tempList = new List<string>();
        tempList.AddRange(totalInputElementList);
        for (int i = 0; i < tempList.Count / 7; i++)
        {
            if (i == 0)
            {
                tempList.Insert(0, $"element[{i}]:");
                continue;
            }

            if (tempList[i].StartsWith("SemanticName: "))
            {
                tempList.Insert(i + 7, $"element[{i}]:");
            }
        }

        return tempList;
    }

    private List<string> GetIBFormat()
    {
        List<string> ibFormat = new List<string>();
        foreach (var ibHash in ibTxtFiles.Keys)
        {
            using var fs = new StreamHelper().GetFileStream(Path.Combine(frameAnalysisPath, ibTxtFiles[ibHash][0]));
            var sr = new StreamReader(fs);
            string topology = "";
            for (int i = 0; i < 4; i++)
            {
                topology = sr.ReadLine();
            }

            ibFormat.Add(topology);
            string dataFormat = sr.ReadLine();
            ibFormat.Add(dataFormat);
        }
        
        return ibFormat;
    }
}
