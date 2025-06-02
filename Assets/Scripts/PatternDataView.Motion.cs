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
    public unsafe MessageSpecs.MotionTarget MotionTarget
      { get => (MessageSpecs.MotionTarget)Motion.partSlots[MotionSelect - 1];
        set => Motion.partSlots[MotionSelect - 1] = (byte)value; }

    [CreateProperty]
    public unsafe MessageSpecs.MotionDest MotionDest
      { get => (MessageSpecs.MotionDest)Motion.destinations[MotionSelect - 1];
        set => Motion.destinations[MotionSelect - 1] = (byte)value; }

    int MotionValueOffset => (MotionSelect - 1)  * 64;

    [CreateProperty]
    public unsafe int MotionValue1
      { get => Motion.sequence[MotionValueOffset + 0];
        set => Motion.sequence[MotionValueOffset + 0] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue2
      { get => Motion.sequence[MotionValueOffset + 1];
        set => Motion.sequence[MotionValueOffset + 1] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue3
      { get => Motion.sequence[MotionValueOffset + 2];
        set => Motion.sequence[MotionValueOffset + 2] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue4
      { get => Motion.sequence[MotionValueOffset + 3];
        set => Motion.sequence[MotionValueOffset + 3] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue5
      { get => Motion.sequence[MotionValueOffset + 4];
        set => Motion.sequence[MotionValueOffset + 4] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue6
      { get => Motion.sequence[MotionValueOffset + 5];
        set => Motion.sequence[MotionValueOffset + 5] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue7
      { get => Motion.sequence[MotionValueOffset + 6];
        set => Motion.sequence[MotionValueOffset + 6] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue8
      { get => Motion.sequence[MotionValueOffset + 7];
        set => Motion.sequence[MotionValueOffset + 7] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue9
      { get => Motion.sequence[MotionValueOffset + 8];
        set => Motion.sequence[MotionValueOffset + 8] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue10
      { get => Motion.sequence[MotionValueOffset + 9];
        set => Motion.sequence[MotionValueOffset + 9] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue11
      { get => Motion.sequence[MotionValueOffset + 10];
        set => Motion.sequence[MotionValueOffset + 10] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue12
      { get => Motion.sequence[MotionValueOffset + 11];
        set => Motion.sequence[MotionValueOffset + 11] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue13
      { get => Motion.sequence[MotionValueOffset + 12];
        set => Motion.sequence[MotionValueOffset + 12] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue14
      { get => Motion.sequence[MotionValueOffset + 13];
        set => Motion.sequence[MotionValueOffset + 13] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue15
      { get => Motion.sequence[MotionValueOffset + 14];
        set => Motion.sequence[MotionValueOffset + 14] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue16
      { get => Motion.sequence[MotionValueOffset + 15];
        set => Motion.sequence[MotionValueOffset + 15] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue17
      { get => Motion.sequence[MotionValueOffset + 16];
        set => Motion.sequence[MotionValueOffset + 16] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue18
      { get => Motion.sequence[MotionValueOffset + 17];
        set => Motion.sequence[MotionValueOffset + 17] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue19
      { get => Motion.sequence[MotionValueOffset + 18];
        set => Motion.sequence[MotionValueOffset + 18] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue20
      { get => Motion.sequence[MotionValueOffset + 19];
        set => Motion.sequence[MotionValueOffset + 19] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue21
      { get => Motion.sequence[MotionValueOffset + 20];
        set => Motion.sequence[MotionValueOffset + 20] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue22
      { get => Motion.sequence[MotionValueOffset + 21];
        set => Motion.sequence[MotionValueOffset + 21] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue23
      { get => Motion.sequence[MotionValueOffset + 22];
        set => Motion.sequence[MotionValueOffset + 22] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue24
      { get => Motion.sequence[MotionValueOffset + 23];
        set => Motion.sequence[MotionValueOffset + 23] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue25
      { get => Motion.sequence[MotionValueOffset + 24];
        set => Motion.sequence[MotionValueOffset + 24] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue26
      { get => Motion.sequence[MotionValueOffset + 25];
        set => Motion.sequence[MotionValueOffset + 25] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue27
      { get => Motion.sequence[MotionValueOffset + 26];
        set => Motion.sequence[MotionValueOffset + 26] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue28
      { get => Motion.sequence[MotionValueOffset + 27];
        set => Motion.sequence[MotionValueOffset + 27] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue29
      { get => Motion.sequence[MotionValueOffset + 28];
        set => Motion.sequence[MotionValueOffset + 28] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue30
      { get => Motion.sequence[MotionValueOffset + 29];
        set => Motion.sequence[MotionValueOffset + 29] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue31
      { get => Motion.sequence[MotionValueOffset + 30];
        set => Motion.sequence[MotionValueOffset + 30] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue32
      { get => Motion.sequence[MotionValueOffset + 31];
        set => Motion.sequence[MotionValueOffset + 31] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue33
      { get => Motion.sequence[MotionValueOffset + 32];
        set => Motion.sequence[MotionValueOffset + 32] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue34
      { get => Motion.sequence[MotionValueOffset + 33];
        set => Motion.sequence[MotionValueOffset + 33] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue35
      { get => Motion.sequence[MotionValueOffset + 34];
        set => Motion.sequence[MotionValueOffset + 34] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue36
      { get => Motion.sequence[MotionValueOffset + 35];
        set => Motion.sequence[MotionValueOffset + 35] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue37
      { get => Motion.sequence[MotionValueOffset + 36];
        set => Motion.sequence[MotionValueOffset + 36] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue38
      { get => Motion.sequence[MotionValueOffset + 37];
        set => Motion.sequence[MotionValueOffset + 37] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue39
      { get => Motion.sequence[MotionValueOffset + 38];
        set => Motion.sequence[MotionValueOffset + 38] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue40
      { get => Motion.sequence[MotionValueOffset + 39];
        set => Motion.sequence[MotionValueOffset + 39] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue41
      { get => Motion.sequence[MotionValueOffset + 40];
        set => Motion.sequence[MotionValueOffset + 40] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue42
      { get => Motion.sequence[MotionValueOffset + 41];
        set => Motion.sequence[MotionValueOffset + 41] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue43
      { get => Motion.sequence[MotionValueOffset + 42];
        set => Motion.sequence[MotionValueOffset + 42] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue44
      { get => Motion.sequence[MotionValueOffset + 43];
        set => Motion.sequence[MotionValueOffset + 43] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue45
      { get => Motion.sequence[MotionValueOffset + 44];
        set => Motion.sequence[MotionValueOffset + 44] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue46
      { get => Motion.sequence[MotionValueOffset + 45];
        set => Motion.sequence[MotionValueOffset + 45] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue47
      { get => Motion.sequence[MotionValueOffset + 46];
        set => Motion.sequence[MotionValueOffset + 46] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue48
      { get => Motion.sequence[MotionValueOffset + 47];
        set => Motion.sequence[MotionValueOffset + 47] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue49
      { get => Motion.sequence[MotionValueOffset + 48];
        set => Motion.sequence[MotionValueOffset + 48] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue50
      { get => Motion.sequence[MotionValueOffset + 49];
        set => Motion.sequence[MotionValueOffset + 49] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue51
      { get => Motion.sequence[MotionValueOffset + 50];
        set => Motion.sequence[MotionValueOffset + 50] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue52
      { get => Motion.sequence[MotionValueOffset + 51];
        set => Motion.sequence[MotionValueOffset + 51] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue53
      { get => Motion.sequence[MotionValueOffset + 52];
        set => Motion.sequence[MotionValueOffset + 52] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue54
      { get => Motion.sequence[MotionValueOffset + 53];
        set => Motion.sequence[MotionValueOffset + 53] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue55
      { get => Motion.sequence[MotionValueOffset + 54];
        set => Motion.sequence[MotionValueOffset + 54] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue56
      { get => Motion.sequence[MotionValueOffset + 55];
        set => Motion.sequence[MotionValueOffset + 55] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue57
      { get => Motion.sequence[MotionValueOffset + 56];
        set => Motion.sequence[MotionValueOffset + 56] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue58
      { get => Motion.sequence[MotionValueOffset + 57];
        set => Motion.sequence[MotionValueOffset + 57] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue59
      { get => Motion.sequence[MotionValueOffset + 58];
        set => Motion.sequence[MotionValueOffset + 58] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue60
      { get => Motion.sequence[MotionValueOffset + 59];
        set => Motion.sequence[MotionValueOffset + 59] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue61
      { get => Motion.sequence[MotionValueOffset + 60];
        set => Motion.sequence[MotionValueOffset + 60] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue62
      { get => Motion.sequence[MotionValueOffset + 61];
        set => Motion.sequence[MotionValueOffset + 61] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue63
      { get => Motion.sequence[MotionValueOffset + 62];
        set => Motion.sequence[MotionValueOffset + 62] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue64
      { get => Motion.sequence[MotionValueOffset + 63];
        set => Motion.sequence[MotionValueOffset + 63] = (byte)value; }

    #endregion
}
