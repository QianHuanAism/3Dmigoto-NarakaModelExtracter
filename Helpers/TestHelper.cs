using NMC.Model;
using System.Collections.ObjectModel;

namespace NMC.Helpers;

public class TestHelper
{
    public string ExtractTest(ObservableCollection<DrawIB> ibList)
    {
        ibList.Add(new DrawIB
        {
            IBHash = "6b79db0e"
        });

        return "E:\\XXMI Launcher\\GIMI\\FrameAnalysis-2026-01-15-214756";
    }
}
