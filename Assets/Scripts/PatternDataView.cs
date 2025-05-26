using System;
using System.Runtime.InteropServices;

public sealed partial class PatternDataView
{
    #region Public methods

    public ReadOnlySpan<byte> RawData
      => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref _data, 1));

    public void UpdateData(ReadOnlySpan<byte> source)
      => _data = MemoryMarshal.Cast<byte, MessageSpecs.Pattern>(source)[0];

    public ref MessageSpecs.Part CurrentPart
      => ref GetPartRef(PartSelect - 1);

    public ref MessageSpecs.Step CurrentStep
      => ref GetStepRef(PartSelect - 1, StepSelect - 1);

    public ref MessageSpecs.Part GetPartRef(int part)
    {
        var source = MemoryMarshal.CreateSpan(ref _data, 1);
        var offs = 2048 + 816 * part;
        var span = MemoryMarshal.AsBytes(source).Slice(offs, 816);
        return ref MemoryMarshal.Cast<byte, MessageSpecs.Part>(span)[0];
    }

    public ref MessageSpecs.Step GetStepRef(int part, int step)
    {
        var source = MemoryMarshal.CreateSpan(ref _data, 1);
        var offs = 2048 + 816 * part + 48 + 12 * step;
        var span = MemoryMarshal.AsBytes(source).Slice(offs, 12);
        return ref MemoryMarshal.Cast<byte, MessageSpecs.Step>(span)[0];
    }

    #endregion

    #region Raw data

    MessageSpecs.Pattern _data;

    #endregion
}
