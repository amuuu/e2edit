using Unity.Properties;
using System.Runtime.InteropServices;

public sealed partial class PatternDataView
{
    #region Master FX data window

    public ref MessageSpecs.MasterFX MasterFX => ref GetMasterFXRef();

    ref MessageSpecs.MasterFX GetMasterFXRef()
    {
        var source = MemoryMarshal.CreateSpan(ref Data, 1);
        var span = MemoryMarshal.AsBytes(source).Slice(58, 8);
        return ref MemoryMarshal.Cast<byte, MessageSpecs.MasterFX>(span)[0];
    }

    [CreateProperty]
    public int MfxType
      { get => MasterFX.type + 1;
        set => MasterFX.type = (byte)(value - 1); }

    [CreateProperty]
    public string MfxTypeName
      => GlobalStringTable.MfxTypes[MfxType - 1];

    [CreateProperty]
    public int MfxPadX
      { get => MasterFX.padX;
        set => MasterFX.padX = (byte)value; }

    [CreateProperty]
    public int MfxPadY
      { get => MasterFX.padY;
        set => MasterFX.padY = (byte)value; }

    [CreateProperty]
    public bool MfxHold
      { get => MasterFX.hold != 0;
        set => MasterFX.hold = (byte)(value ? 1 : 0); }

    #endregion
}
