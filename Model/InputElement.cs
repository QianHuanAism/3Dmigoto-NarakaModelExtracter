using System;
using System.Collections.Generic;
using System.Text;

namespace NMC.Model;

public class InputElement
{
    public string SemanticName { get; set; }
    public string SemanticIndex { get; set; }
    public string Format { get; set; }
    public string InputSlot { get; set; }
    public string AlignedByteOffset { get; set; }
    public string InputSlotClass { get; set; }
    public string InstanceDataStepRate { get; set; }
}
