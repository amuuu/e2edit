using System;
using System.Runtime.InteropServices;

public sealed class PatternDataView
{
    public enum BeatType { _16, _32, _8Tri, _16Tri }

    #region Field accessors

    public string PatternName
      { get => GetPatternName();
        set => SetPatternName(value); }

    public float Tempo
      { get => _data.tempo * 0.1f;
        set => _data.tempo = (ushort)(value * 10); }

    public float Swing
      { get => _data.swing / 48.0f * 50;
        set => _data.swing = (sbyte)(value * 48 / 50); }

    public int Length
      { get => _data.length + 1;
        set => _data.length = (byte)(value - 1); }

    public BeatType Beat
      { get => (BeatType)_data.beat;
        set => _data.beat = (byte)(value); }

    public int PlayLevel
      { get => 127 - _data.playLevel;
        set => _data.playLevel = (byte)(127 - value); }

    #endregion

    #region Public methods

    public void UpdateData(ReadOnlySpan<byte> source)
      => _data = MemoryMarshal.Cast<byte, MessageSpec.Pattern>(source)[0];

    #endregion

    #region Raw data

    MessageSpec.Pattern _data;

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
        while (i < len) _data.patternName[i] = (byte)name[i];
        _data.patternName[i] = 0;
    }

    #endregion
}
