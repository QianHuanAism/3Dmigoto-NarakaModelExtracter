using Dumpify;
using NMC.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace NMC.Helpers;

public record VBAnalyzer(string frameAnalysis, Dictionary<string, List<string>> vbFiles)
{
    private StreamBuilder streamBuilder = new StreamBuilder();
    private VBStrideCollector strideCollector = new VBStrideCollector(frameAnalysis, vbFiles);
    private VBVertexCountCollector vertexCountCollector = new VBVertexCountCollector(frameAnalysis, vbFiles);
    private VBSemanticCollector semanticCollector = new VBSemanticCollector(frameAnalysis, vbFiles);

    public void Analyze()
    {
        var strides = strideCollector.GetStride();
        var vertexCounts = vertexCountCollector.GetVBVertexCount();
        var semanticNameList = semanticCollector.GetValidSemantic();
    }
}
