using Dumpify;
using System.IO;
using System.Windows.Controls.Ribbon;

namespace NMC.Helpers;

public class DrawCallCollector
{
    public List<string>? CollectIBDrawCall(string frameAnalysis, string ibHash)
    {
        List<string>? drawCallList = new List<string>();
        foreach (var file in Directory.GetFiles(frameAnalysis).ToList())
        {
            if (file.Contains($"-ib={ibHash}"))
            {
                string drawCall = Path.GetFileName(file.Split($"-ib={ibHash}")[0]);
                if(!drawCallList.Contains(drawCall))
                    drawCallList.Add(drawCall);
            }
        }

        if (drawCallList.Count <= 0)
        {
            return null;
        }
        
        return drawCallList;
    }
}