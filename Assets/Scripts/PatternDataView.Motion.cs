using Unity.Properties;
using System.Runtime.InteropServices;

public sealed partial class PatternDataView
{
    #region Motion sequence data window

    [CreateProperty]
    public int MotionSelect { get; set; }

    public ref MessageSpecs.Motion Motion
      => ref GetMotionRef();

    public ref MessageSpecs.Motion GetMotionRef()
    {
        var source = MemoryMarshal.CreateSpan(ref Data, 1);
        var span = MemoryMarshal.AsBytes(source).Slice(256, 1584);
        return ref MemoryMarshal.Cast<byte, MessageSpecs.Motion>(span)[0];
    }

    [CreateProperty]
    public unsafe MessageSpecs.MotionTarget MotionTarget
      { get => (MessageSpecs.MotionTarget)Motion.partSlots[MotionSelect];
        set => Motion.partSlots[MotionSelect] = (byte)value; }

    [CreateProperty]
    public unsafe MessageSpecs.MotionDest MotionDest
      { get => (MessageSpecs.MotionDest)Motion.destinations[MotionSelect];
        set => Motion.destinations[MotionSelect] = (byte)value; }

    #endregion
}
