using System;
using System.Runtime.InteropServices;

public sealed partial class PatternDataView
{
    #region Target data accessors

    // Access as struct
    public MessageSpecs.Pattern Data;

    // Access as byte span
    public Span<byte> AsBytes
      => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref Data, 1));

    #endregion
}
