using Dumpify;
using NMC.Core;
using NMC.Helpers;
using NMC.Model;
using NMC.Utils;
using System.IO;
using System.Windows;

namespace NMC.Services;

public class ModelExtractionService : IModelExtractionService
{
    private VBStrideCollector strideCollector = new VBStrideCollector();
    private VBVertexCountCollector vertexCountCollector = new VBVertexCountCollector();
    private VBSemanticCollector semanticCollector = new VBSemanticCollector();
    private VBSemanticBlockCollector semanticBlockCollector = new VBSemanticBlockCollector();
    private string? frameAnalysisPath;

    public ModelExtractionService(string frameAnalysisPath)
    {
        this.frameAnalysisPath = frameAnalysisPath;
    }

    public void Extract(string ibHash)
    {
        // 考虑到可能有人在添加了以后又会删除, 所以可能即使通过了断言, 但是其实还是会有空字符串, 所以这里做二次判断  
        if (string.IsNullOrEmpty(ibHash))
        {
            return;
        }
        Log.Info($"开始收集DrawIB: {ibHash} 的绘制调用");
        // 用于存储当前 IB 对应的 DrawCall
        Dictionary<string, List<string>> ibDrawCallMap = new Dictionary<string, List<string>>();
        DrawCallCollector drawCallCollector = new DrawCallCollector();
        List<string>? ibDrawCallList = drawCallCollector.CollectIBDrawCall(frameAnalysisPath!, ibHash);
        if (ibDrawCallList == null)
        {
            Log.Info($"{ibHash} 对应的 DrawCall 均为空, 开始收集下一个 DrawIB");
            return;
        }
        ibDrawCallList.ForEach(dc => Log.Info($"绘制调用: {dc}"));
        ibDrawCallMap.Add(ibHash, ibDrawCallList);
        Log.Info($"DrawIB: {ibHash} 的绘制调用收集结束");
        Log.Info("-", 66);


        Log.Info("开始收集绘制调用对应的 VB 文件");
        Dictionary<string, List<string>>? vbTxtFiles = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>>? vbBufFiles = new Dictionary<string, List<string>>();
        VBCollector vbCollector = new VBCollector(frameAnalysisPath!, ibDrawCallMap);
        vbTxtFiles = vbCollector.CollectTxtVBFile();
        vbBufFiles = vbCollector.CollectBufVBFile();
        if (vbTxtFiles == null || vbBufFiles == null)
        {
            MessageHelper.Show($"未找到绘制调用对应的任何 VB 文件, 提取失败!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);            
            return;
        }
        Log.Info("收集绘制调用对应的 VB 文件结束");
        Log.Info("-", 66);
        
        Log.Info($"开始分析当前 DrawIB {ibHash} 对应的 VB 文件");
        
        Dictionary<string, string>? strides = strideCollector.GetStride(frameAnalysisPath!, vbTxtFiles);        
        Dictionary<string, string>? vertexCounts = vertexCountCollector.GetVBVertexCount(frameAnalysisPath!, vbTxtFiles);
        List<Dictionary<string, Dictionary<string, string>>>? semanticList = semanticCollector.GetValidSemantic(frameAnalysisPath!, vbTxtFiles);
        List<Dictionary<string, List<string>>>? semanticBlockList = semanticBlockCollector.GetValidSemanticBlock(frameAnalysisPath!, vbTxtFiles, semanticList);

        Log.Info($"当前 DrawIB {ibHash} 对应的 VB 文件分析结束");
        Log.Info("-", 66);

        Log.Info($"开始分析 \"log.txt\" 文件");
        string logFilePath = Path.Combine(frameAnalysisPath!, "log.txt");
        VBHashCollector vbHashCollector = new VBHashCollector();
        Dictionary<string, string>? drawCallToVBHashMap = vbHashCollector.CollectVB0Hash(vbTxtFiles);
        LogFileAnalyzer logFileAnalyzer = new LogFileAnalyzer(logFilePath);
        List<string> cstFileList = new List<string>();
        cstFileList = logFileAnalyzer.AnalyzeLog(drawCallToVBHashMap);
        Log.Info($"分析 \"log.txt\" 文件结束");
        Log.Info("-", 66);
    }
}
