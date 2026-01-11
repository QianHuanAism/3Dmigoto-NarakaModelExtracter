using Dumpify;
using NMC.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NMC.Helpers;

public record VBSemanticCollector(string frameAnalysis, Dictionary<string, List<string>> vbFiles)
{
    private StreamBuilder streamBuilder = new StreamBuilder();

    public object GetValidSemantic()
    {
        throw new NotImplementedException();
    }
}
