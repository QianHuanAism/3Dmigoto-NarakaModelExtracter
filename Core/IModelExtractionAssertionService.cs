using NMC.Model;
using System.Collections.ObjectModel;

namespace NMC.Core;

public interface IModelExtractionAssertionService
{
    bool CanExtract(
        FrameAnalysis frameAnalysis,
        ObservableCollection<DrawIB> drawIBList,
        Output output
    );
}
