using CommunityToolkit.Mvvm.ComponentModel;

namespace NMC.Model;

public partial class Output : ObservableObject
{
    [ObservableProperty]
    private string? _outputPath;
}
