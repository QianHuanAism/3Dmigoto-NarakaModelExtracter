using System.Text.RegularExpressions;

namespace NMC.Utils.VB;

public class VBHashCollector
{
    public Dictionary<string, string> CollectVB0Hash(Dictionary<string, List<string>> vbFiles)
    {
        Dictionary<string, string> drawCallToVBHashMap = new Dictionary<string, string>();
        foreach (var ibHash in vbFiles.Keys)
        {
            foreach (var file in vbFiles[ibHash])
            {
                // 找出VB0文件
                string pattern = @"^\d{6}-vb0=[0-9a-zA-Z]{8}";
                bool isSuccess = Regex.Match(file, pattern).Success;
                if (isSuccess)
                {
                    var drawCall = Regex.Match(file, pattern).Value.Split("-vb0=")[0];
                    var vbHash = Regex.Match(file, pattern).Value.Split("-vb0=")[1];

                    drawCallToVBHashMap.Add(drawCall, vbHash);
                }
            }
        }
        return drawCallToVBHashMap;
    }
}
