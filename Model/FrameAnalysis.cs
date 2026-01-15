using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;

namespace NMC.Model;

public partial class FrameAnalysis : ObservableObject
{
    [ObservableProperty]
    private string? _frameAnalysisPath;

    public bool IsValid => string.IsNullOrEmpty(FrameAnalysisPath) || !Directory.Exists(FrameAnalysisPath);
}
