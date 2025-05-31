using Unity.Properties;
using System.Runtime.InteropServices;
using System;

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
        set => Motion.partSlots[MotionSelect] = (byte)value; }

    [CreateProperty]
    public unsafe MessageSpecs.MotionDest MotionDest
      { get => (MessageSpecs.MotionDest)Motion.destinations[MotionSelect - 1];
        set => Motion.destinations[MotionSelect] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue1
      { get => Motion.sequence[MotionSelect * 64 + 0];
        set => Motion.sequence[MotionSelect * 64 + 0] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue2
      { get => Motion.sequence[MotionSelect * 64 + 1];
        set => Motion.sequence[MotionSelect * 64 + 1] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue3
      { get => Motion.sequence[MotionSelect * 64 + 2];
        set => Motion.sequence[MotionSelect * 64 + 2] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue4
      { get => Motion.sequence[MotionSelect * 64 + 3];
        set => Motion.sequence[MotionSelect * 64 + 3] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue5
      { get => Motion.sequence[MotionSelect * 64 + 4];
        set => Motion.sequence[MotionSelect * 64 + 4] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue6
      { get => Motion.sequence[MotionSelect * 64 + 5];
        set => Motion.sequence[MotionSelect * 64 + 5] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue7
      { get => Motion.sequence[MotionSelect * 64 + 6];
        set => Motion.sequence[MotionSelect * 64 + 6] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue8
      { get => Motion.sequence[MotionSelect * 64 + 7];
        set => Motion.sequence[MotionSelect * 64 + 7] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue9
      { get => Motion.sequence[MotionSelect * 64 + 8];
        set => Motion.sequence[MotionSelect * 64 + 8] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue10
      { get => Motion.sequence[MotionSelect * 64 + 9];
        set => Motion.sequence[MotionSelect * 64 + 9] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue11
      { get => Motion.sequence[MotionSelect * 64 + 10];
        set => Motion.sequence[MotionSelect * 64 + 10] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue12
      { get => Motion.sequence[MotionSelect * 64 + 11];
        set => Motion.sequence[MotionSelect * 64 + 11] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue13
      { get => Motion.sequence[MotionSelect * 64 + 12];
        set => Motion.sequence[MotionSelect * 64 + 12] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue14
      { get => Motion.sequence[MotionSelect * 64 + 13];
        set => Motion.sequence[MotionSelect * 64 + 13] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue15
      { get => Motion.sequence[MotionSelect * 64 + 14];
        set => Motion.sequence[MotionSelect * 64 + 14] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue16
      { get => Motion.sequence[MotionSelect * 64 + 15];
        set => Motion.sequence[MotionSelect * 64 + 15] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue17
      { get => Motion.sequence[MotionSelect * 64 + 16];
        set => Motion.sequence[MotionSelect * 64 + 16] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue18
      { get => Motion.sequence[MotionSelect * 64 + 17];
        set => Motion.sequence[MotionSelect * 64 + 17] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue19
      { get => Motion.sequence[MotionSelect * 64 + 18];
        set => Motion.sequence[MotionSelect * 64 + 18] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue20
      { get => Motion.sequence[MotionSelect * 64 + 19];
        set => Motion.sequence[MotionSelect * 64 + 19] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue21
      { get => Motion.sequence[MotionSelect * 64 + 20];
        set => Motion.sequence[MotionSelect * 64 + 20] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue22
      { get => Motion.sequence[MotionSelect * 64 + 21];
        set => Motion.sequence[MotionSelect * 64 + 21] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue23
      { get => Motion.sequence[MotionSelect * 64 + 22];
        set => Motion.sequence[MotionSelect * 64 + 22] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue24
      { get => Motion.sequence[MotionSelect * 64 + 23];
        set => Motion.sequence[MotionSelect * 64 + 23] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue25
      { get => Motion.sequence[MotionSelect * 64 + 24];
        set => Motion.sequence[MotionSelect * 64 + 24] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue26
      { get => Motion.sequence[MotionSelect * 64 + 25];
        set => Motion.sequence[MotionSelect * 64 + 25] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue27
      { get => Motion.sequence[MotionSelect * 64 + 26];
        set => Motion.sequence[MotionSelect * 64 + 26] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue28
      { get => Motion.sequence[MotionSelect * 64 + 27];
        set => Motion.sequence[MotionSelect * 64 + 27] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue29
      { get => Motion.sequence[MotionSelect * 64 + 28];
        set => Motion.sequence[MotionSelect * 64 + 28] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue30
      { get => Motion.sequence[MotionSelect * 64 + 29];
        set => Motion.sequence[MotionSelect * 64 + 29] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue31
      { get => Motion.sequence[MotionSelect * 64 + 30];
        set => Motion.sequence[MotionSelect * 64 + 30] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue32
      { get => Motion.sequence[MotionSelect * 64 + 31];
        set => Motion.sequence[MotionSelect * 64 + 31] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue33
      { get => Motion.sequence[MotionSelect * 64 + 32];
        set => Motion.sequence[MotionSelect * 64 + 32] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue34
      { get => Motion.sequence[MotionSelect * 64 + 33];
        set => Motion.sequence[MotionSelect * 64 + 33] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue35
      { get => Motion.sequence[MotionSelect * 64 + 34];
        set => Motion.sequence[MotionSelect * 64 + 34] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue36
      { get => Motion.sequence[MotionSelect * 64 + 35];
        set => Motion.sequence[MotionSelect * 64 + 35] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue37
      { get => Motion.sequence[MotionSelect * 64 + 36];
        set => Motion.sequence[MotionSelect * 64 + 36] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue38
      { get => Motion.sequence[MotionSelect * 64 + 37];
        set => Motion.sequence[MotionSelect * 64 + 37] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue39
      { get => Motion.sequence[MotionSelect * 64 + 38];
        set => Motion.sequence[MotionSelect * 64 + 38] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue40
      { get => Motion.sequence[MotionSelect * 64 + 39];
        set => Motion.sequence[MotionSelect * 64 + 39] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue41
      { get => Motion.sequence[MotionSelect * 64 + 40];
        set => Motion.sequence[MotionSelect * 64 + 40] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue42
      { get => Motion.sequence[MotionSelect * 64 + 41];
        set => Motion.sequence[MotionSelect * 64 + 41] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue43
      { get => Motion.sequence[MotionSelect * 64 + 42];
        set => Motion.sequence[MotionSelect * 64 + 42] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue44
      { get => Motion.sequence[MotionSelect * 64 + 43];
        set => Motion.sequence[MotionSelect * 64 + 43] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue45
      { get => Motion.sequence[MotionSelect * 64 + 44];
        set => Motion.sequence[MotionSelect * 64 + 44] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue46
      { get => Motion.sequence[MotionSelect * 64 + 45];
        set => Motion.sequence[MotionSelect * 64 + 45] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue47
      { get => Motion.sequence[MotionSelect * 64 + 46];
        set => Motion.sequence[MotionSelect * 64 + 46] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue48
      { get => Motion.sequence[MotionSelect * 64 + 47];
        set => Motion.sequence[MotionSelect * 64 + 47] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue49
      { get => Motion.sequence[MotionSelect * 64 + 48];
        set => Motion.sequence[MotionSelect * 64 + 48] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue50
      { get => Motion.sequence[MotionSelect * 64 + 49];
        set => Motion.sequence[MotionSelect * 64 + 49] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue51
      { get => Motion.sequence[MotionSelect * 64 + 50];
        set => Motion.sequence[MotionSelect * 64 + 50] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue52
      { get => Motion.sequence[MotionSelect * 64 + 51];
        set => Motion.sequence[MotionSelect * 64 + 51] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue53
      { get => Motion.sequence[MotionSelect * 64 + 52];
        set => Motion.sequence[MotionSelect * 64 + 52] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue54
      { get => Motion.sequence[MotionSelect * 64 + 53];
        set => Motion.sequence[MotionSelect * 64 + 53] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue55
      { get => Motion.sequence[MotionSelect * 64 + 54];
        set => Motion.sequence[MotionSelect * 64 + 54] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue56
      { get => Motion.sequence[MotionSelect * 64 + 55];
        set => Motion.sequence[MotionSelect * 64 + 55] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue57
      { get => Motion.sequence[MotionSelect * 64 + 56];
        set => Motion.sequence[MotionSelect * 64 + 56] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue58
      { get => Motion.sequence[MotionSelect * 64 + 57];
        set => Motion.sequence[MotionSelect * 64 + 57] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue59
      { get => Motion.sequence[MotionSelect * 64 + 58];
        set => Motion.sequence[MotionSelect * 64 + 58] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue60
      { get => Motion.sequence[MotionSelect * 64 + 59];
        set => Motion.sequence[MotionSelect * 64 + 59] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue61
      { get => Motion.sequence[MotionSelect * 64 + 60];
        set => Motion.sequence[MotionSelect * 64 + 60] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue62
      { get => Motion.sequence[MotionSelect * 64 + 61];
        set => Motion.sequence[MotionSelect * 64 + 61] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue63
      { get => Motion.sequence[MotionSelect * 64 + 62];
        set => Motion.sequence[MotionSelect * 64 + 62] = (byte)value; }

    [CreateProperty]
    public unsafe int MotionValue64
      { get => Motion.sequence[MotionSelect * 64 + 63];
        set => Motion.sequence[MotionSelect * 64 + 63] = (byte)value; }

    #endregion
}
