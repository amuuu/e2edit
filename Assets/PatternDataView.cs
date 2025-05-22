using System;
using System.Runtime.InteropServices;

public sealed class PatternDataView
{
    #region Field accessors

    public string PatternName
      { get => GetPatternName();
        set => SetPatternName(value); }

    public float Tempo
      { get => _data.tempo * 0.1f;
        set => _data.tempo = (ushort)(value * 10); }

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
