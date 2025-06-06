using Unity.Properties;
using System.Runtime.InteropServices;

public sealed partial class PatternDataView
{
    #region Part data window

    [CreateProperty]
    public int PartSelect { get; set; } = 1;

    public ref MessageSpecs.Part CurrentPart
      => ref GetPartRef(PartSelect - 1);

    public ref MessageSpecs.Part GetPartRef(int part)
    {
        var source = MemoryMarshal.CreateSpan(ref Data, 1);
        var offs = 2048 + 816 * part;
        var span = MemoryMarshal.AsBytes(source).Slice(offs, 816);
        return ref MemoryMarshal.Cast<byte, MessageSpecs.Part>(span)[0];
    }

    #endregion

    #region Current part data accessors

    [CreateProperty]
    public int LastStep
      { get => CurrentPart.lastStep == 0 ? 16 : CurrentPart.lastStep;
        set => CurrentPart.lastStep = (byte)(value == 0 ? 16 : value); }

    [CreateProperty]
    public bool Mute
      { get => CurrentPart.mute > 0;
        set => CurrentPart.mute = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public MessageSpecs.VoiceType VoiceType
      { get => (MessageSpecs.VoiceType)CurrentPart.voiceAssign;
        set => CurrentPart.voiceAssign = (byte)value; }

    [CreateProperty]
    public MessageSpecs.MotionType MotionType
      { get => (MessageSpecs.MotionType)CurrentPart.motionSequence;
        set => CurrentPart.motionSequence = (byte)value; }

    [CreateProperty]
    public bool TrigPadVelocity
      { get => CurrentPart.trigPadVelocity > 0;
        set => CurrentPart.trigPadVelocity = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public bool ScaleMode
      { get => CurrentPart.scaleMode > 0;
        set => CurrentPart.scaleMode = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public bool PartPriority
      { get => CurrentPart.partPriority > 0;
        set => CurrentPart.partPriority = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public int OscillatorType
      { get => CurrentPart.oscillatorType + 1;
        set => CurrentPart.oscillatorType = (ushort)(value - 1); }

    [CreateProperty]
    public string OscillatorTypeName
      => StringTable.Instance.oscillatorTypes[OscillatorType - 1];

    [CreateProperty]
    public string OscillatorTypeCategory
      => StringTable.Instance.oscillatorCategories[OscillatorType - 1];

    [CreateProperty]
    public int OscillatorEdit
      { get => CurrentPart.oscillatorEdit;
        set => CurrentPart.oscillatorEdit = (byte)value; }

    [CreateProperty]
    public int FilterType
      { get => CurrentPart.filterType;
        set => CurrentPart.filterType = (byte)value; }

    [CreateProperty]
    public int FilterCutoff
      { get => CurrentPart.filterCutoff;
        set => CurrentPart.filterCutoff = (byte)value; }

    [CreateProperty]
    public int FilterResonance
      { get => CurrentPart.filterResonance;
        set => CurrentPart.filterResonance = (byte)value; }

    [CreateProperty]
    public int FilterEgInt
      { get => CurrentPart.filterEgInt;
        set => CurrentPart.filterEgInt = (sbyte)value; }

    [CreateProperty]
    public int ModulationType
      { get => CurrentPart.modulationType + 1;
        set => CurrentPart.modulationType = (byte)(value - 1); }

    [CreateProperty]
    public string ModulationTypeName
      => StringTable.Instance.modulationTypes[ModulationType - 1];

    [CreateProperty]
    public int ModulationSpeed
      { get => CurrentPart.modulationSpeed;
        set => CurrentPart.modulationSpeed = (byte)value; }

    [CreateProperty]
    public int ModulationDepth
      { get => CurrentPart.modulationDepth;
        set => CurrentPart.modulationDepth = (byte)value; }

    [CreateProperty]
    public int EgAttack
      { get => CurrentPart.egAttack;
        set => CurrentPart.egAttack = (byte)value; }

    [CreateProperty]
    public int EgDecayRelease
      { get => CurrentPart.egDecayRelease;
        set => CurrentPart.egDecayRelease = (byte)value; }

    [CreateProperty]
    public int AmpLevel
      { get => CurrentPart.ampLevel;
        set => CurrentPart.ampLevel = (byte)value; }

    [CreateProperty]
    public int AmpPan
      { get => CurrentPart.ampPan;
        set => CurrentPart.ampPan = (sbyte)value; }

    [CreateProperty]
    public bool EgOnOff
      { get => CurrentPart.egOnOff > 0;
        set => CurrentPart.egOnOff = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public bool MfxSendOnOff
      { get => CurrentPart.mfxSendOnOff > 0;
        set => CurrentPart.mfxSendOnOff = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public int GrooveType
      { get => CurrentPart.grooveType;
        set => CurrentPart.grooveType = (byte)value; }

    [CreateProperty]
    public int GrooveDepth
      { get => CurrentPart.grooveDepth;
        set => CurrentPart.grooveDepth = (byte)value; }

    [CreateProperty]
    public bool IfxOnOff
      { get => CurrentPart.ifxOnOff > 0;
        set => CurrentPart.ifxOnOff = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public int IfxType
      { get => CurrentPart.ifxType;
        set => CurrentPart.ifxType = (byte)value; }

    [CreateProperty]
    public int IfxEdit
      { get => CurrentPart.ifxEdit;
        set => CurrentPart.ifxEdit = (byte)value; }

    [CreateProperty]
    public int OscillatorPitch
      { get => CurrentPart.oscillatorPitch;
        set => CurrentPart.oscillatorPitch = (sbyte)value; }

    [CreateProperty]
    public int OscillatorGlide
      { get => CurrentPart.oscillatorGlide;
        set => CurrentPart.oscillatorGlide = (byte)value; }

    #endregion
}
