using Unity.Properties;

public sealed partial class PatternDataView
{
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
    public MessageSpecs.Beat Beat
      { get => (MessageSpecs.Beat)_data.beat;
        set => _data.beat = (byte)(value); }

    [CreateProperty]
    public int PlayLevel
      { get => 127 - _data.playLevel;
        set => _data.playLevel = (byte)(127 - value); }

    #endregion

    #region Helper methods

    unsafe string GetPatternName()
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

    unsafe void SetPatternName(string name)
    {
        var (i, len) = (0, System.Math.Min(17, name.Length));
        for (;i < len; i++) _data.patternName[i] = (byte)name[i];
        _data.patternName[i] = 0;
    }

    #endregion
}
