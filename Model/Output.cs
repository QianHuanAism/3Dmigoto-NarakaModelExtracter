using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMC.Model;

public partial class Output : ObservableObject
{
    [ObservableProperty]
    private string? _outputPath;
}
