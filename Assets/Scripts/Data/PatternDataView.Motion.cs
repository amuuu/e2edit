using Unity.Properties;
using System.Runtime.InteropServices;

public sealed partial class PatternDataView
{
    #region Motion sequence data window

    [CreateProperty]
    public int MotionSelect { get; set; } = 1;

    public ref MessageSpecs.Motion Motion
      => ref GetMotionRef();

    public ref MessageSpecs.Motion GetMotionRef()
    {
        var source = MemoryMarshal.CreateSpan(ref Data, 1);
        var span = MemoryMarshal.AsBytes(source).Slice(256, 1584);
        return ref MemoryMarshal.Cast<byte, MessageSpecs.Motion>(span)[0];
    }

    [CreateProperty]
    public unsafe int MotionTarget
      { get => Motion.partSlots[MotionSelect - 1];
        set => Motion.partSlots[MotionSelect - 1] = (byte)value; }

    public unsafe int MotionDestRaw
      { get => Motion.destinations[MotionSelect - 1];
        set => Motion.destinations[MotionSelect - 1] = (byte)value; }

    [CreateProperty]
    public int MotionDest
      { get => MotionDestRaw == 0 ? 0 : MotionDestRaw - 1;
        set => MotionDestRaw = value == 0 ? 0 : value + 1; }

    int MotionValueOffset => (MotionSelect - 1)  * 64;

    public unsafe int GetMotionValue(int index)
      => Motion.sequence[MotionValueOffset + index];

    public unsafe void SetMotionValue(int index, int value)
      => Motion.sequence[MotionValueOffset + index] = (byte)value;

    #endregion
}
