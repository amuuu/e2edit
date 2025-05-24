using System;
using System.Runtime.InteropServices;
using Unity.Properties;

public sealed class PatternDataView
{
    #region Enum definitions

    public enum BeatType { _16thNote, _32ndNote, _8thTriplet, _16thTriplet }

    #endregion

    #region Pattern data accessors

    [CreateProperty]
    public string PatternName
      { get => GetPatternName();
        set => SetPatternName(value); }

    [CreateProperty]
    public float Tempo
      { get => _data.tempo * 0.1f;
        set => _data.tempo = (ushort)(value * 10); }

    [CreateProperty]
    public int Swing
      { get => _data.swing;
        set => _data.swing = (sbyte)(value); }

    [CreateProperty]
    public int Length
      { get => _data.length + 1;
        set => _data.length = (byte)(value - 1); }

    [CreateProperty]
    public BeatType Beat
      { get => (BeatType)_data.beat;
        set => _data.beat = (byte)(value); }

    [CreateProperty]
    public int PlayLevel
      { get => 127 - _data.playLevel;
        set => _data.playLevel = (byte)(127 - value); }

    #endregion

    #region Part data accessors

    public enum VoiceAssignType { Mono1, Mono2, Poly1, Poly2 }
    public enum MotionSequenceType { Off, Smooth, TriggerHold }

    [CreateProperty]
    public int PartSelect { get; set; } = 1;

    [CreateProperty]
    public int LastStep
      { get => CurrentPart.lastStep == 0 ? 16 : CurrentPart.lastStep;
        set => CurrentPart.lastStep = (byte)(value == 0 ? 16 : value); }

    [CreateProperty]
    public bool Mute
      { get => CurrentPart.mute > 0;
        set => CurrentPart.mute = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public VoiceAssignType VoiceAssign
      { get => (VoiceAssignType)CurrentPart.voiceAssign;
        set => CurrentPart.voiceAssign = (byte)value; }

    [CreateProperty]
    public MotionSequenceType MotionSequence
      { get => (MotionSequenceType)CurrentPart.motionSequence;
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
      { get => CurrentPart.oscillatorType;
        set => CurrentPart.oscillatorType = (ushort)(value); }

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
      { get => CurrentPart.modulationType;
        set => CurrentPart.modulationType = (byte)value; }

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

    #region Public methods

    public ReadOnlySpan<byte> RawData
      => MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref _data, 1));

    public void UpdateData(ReadOnlySpan<byte> source)
      => _data = MemoryMarshal.Cast<byte, MessageSpecs.Pattern>(source)[0];

    public ref MessageSpecs.Part CurrentPart => ref GetPartRef(PartSelect - 1);

    public ref MessageSpecs.Part GetPartRef(int index)
    {
        var source = MemoryMarshal.CreateSpan(ref _data, 1);
        var span = MemoryMarshal.AsBytes(source).Slice(2048 + 816 * index, 816);
        return ref MemoryMarshal.Cast<byte, MessageSpecs.Part>(span)[0];
    }

    #endregion

    #region Raw data

    MessageSpecs.Pattern _data;

    #endregion

    #region Helper methods

    public unsafe string GetPatternName()
    {
        var temp = "";
        for (var i = 0; i < 18; i++)
        {
            var b = _data.patternName[i];
            if (b == 0) break;
            temp += (char)b;
        }
        return temp;
    }

    public unsafe void SetPatternName(string name)
    {
        var (i, len) = (0, System.Math.Min(17, name.Length));
        for (;i < len; i++) _data.patternName[i] = (byte)name[i];
        _data.patternName[i] = 0;
    }

    #endregion
}
