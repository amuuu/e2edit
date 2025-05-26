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
      { get => Data.tempo * 0.1f;
        set => Data.tempo = (ushort)(value * 10); }

    [CreateProperty]
    public int Swing
      { get => Data.swing;
        set => Data.swing = (sbyte)(value); }

    [CreateProperty]
    public int Length
      { get => Data.length + 1;
        set => Data.length = (byte)(value - 1); }

    [CreateProperty]
    public MessageSpecs.Beat Beat
      { get => (MessageSpecs.Beat)Data.beat;
        set => Data.beat = (byte)(value); }

    [CreateProperty]
    public int PlayLevel
      { get => 127 - Data.playLevel;
        set => Data.playLevel = (byte)(127 - value); }

    #endregion

    #region Helper methods

    unsafe string GetPatternName()
    {
        var temp = "";
        for (var i = 0; i < 18; i++)
        {
            var b = Data.patternName[i];
            if (b == 0) break;
            temp += (char)b;
        }
        return temp;
    }

    unsafe void SetPatternName(string name)
    {
        var (i, len) = (0, System.Math.Min(17, name.Length));
        for (;i < len; i++) Data.patternName[i] = (byte)name[i];
        Data.patternName[i] = 0;
    }

    #endregion
}
