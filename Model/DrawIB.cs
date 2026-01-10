using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMC.Model;

public partial class DrawIB : ObservableObject
{
    [ObservableProperty]
    private string _iBHash;
    [ObservableProperty]
    private string _alias;
}
