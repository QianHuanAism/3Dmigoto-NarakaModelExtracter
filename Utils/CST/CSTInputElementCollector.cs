using Dumpify;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMC.Utils.CST;

public class CSTInputElementCollector
{
    public List<Dictionary<string, List<string>>> GetCSTInputElementList(List<string> cstFiles, Dictionary<string, string> cstStrides)
    {
        List<Dictionary<string, List<string>>> cstInputElementList = new List<Dictionary<string, List<string>>>();
        foreach (var cstFile in cstStrides.Keys)
        {
            string cstStride = cstStrides[cstFile];
            Dictionary<string, List<string>> fileInputElementMap = new Dictionary<string, List<string>>();
            switch (cstStride)
            {
                case "32":
                    fileInputElementMap.Add(cstFile, GetCSTInputElement());
                    break;
            }
            if (!(fileInputElementMap.Count <= 0))
                cstInputElementList.Add(fileInputElementMap);
        }

        return cstInputElementList;
    }

    // 但是这里只获取cs-t1, 因为cs-t0的输入列表已经获取好了, 可以不用获取
    private List<string> GetCSTInputElement()
    {
        List<string> inputElementList = new List<string>();

        Dictionary<string, string> blendFormatMap = new Dictionary<string, string>()
        {
            { "BLENDERWEIGHTS", "R32G32B32A32_FLOAT" },
            { "BLENDERINDICES", "R32G32B32A32_UINT" }
        };

        foreach (var attributeName in blendFormatMap.Keys)
        {
            Dictionary<string, string> attributeFormatMap = new Dictionary<string, string>()
        {
            {"SemanticName", $"{attributeName}" },
            {"SemanticIndex", "0" },
            {"Format", $"{blendFormatMap[attributeName]}" },
            {"InputSlot", "0" },
            {"AlignedByteOffset", "0" },
            {"InputSlotClass", "per-vertex" },
            {"InstanceDataStepRate", "0" },
        };

            foreach (var attribute in attributeFormatMap.Keys)
            {
                inputElementList.Add($"{attribute}: {attributeFormatMap[attribute]}");
            }
        }

        return inputElementList;
    }
}
