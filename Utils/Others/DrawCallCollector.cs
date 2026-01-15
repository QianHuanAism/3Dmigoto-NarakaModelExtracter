using System.IO;

namespace NMC.Utils.Others;

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
                if (!drawCallList.Contains(drawCall))
                    drawCallList.Add(drawCall);
            }
        }

        if (drawCallList.Count <= 0)
        {
            return null;
        }

        return RemoveDuplicateDrawCall(drawCallList);
    }

    // 这里只收集到最先出现的 DrawCall, 其余DrawCall 都可以舍弃
    private List<string> RemoveDuplicateDrawCall(List<string> source)
    {
        List<string> temp = new List<string>();
        foreach (var call in source)
        {
            if (!temp.Contains(call))
            {
                temp.Add(call);
            }
        }

        temp.RemoveRange(1, temp.Count - 1);
        return temp;
    }
}
