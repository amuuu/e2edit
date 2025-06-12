using System;

[Serializable]
public struct StringTable
{
    public string[] oscillatorTypes;
    public string[] oscillatorCategories;
    public string[] modulationTypes;
    public string[] modulationSources;
    public string[] modulationDestinations;
    public string[] filterTypes;
    public string[] scaleTypes;
    public string[] mfxTypes;
    public string[] ifxTypes;
    public string[] grooveTypes;
}

public static class GlobalStringTable
{
    public static ReadOnlySpan<string> OscillatorTypes          => _data.oscillatorTypes.AsSpan();
    public static ReadOnlySpan<string> OscillatorCategories     => _data.oscillatorCategories.AsSpan();
    public static ReadOnlySpan<string> ModulationTypes          => _data.modulationTypes.AsSpan();
    public static ReadOnlySpan<string> ModulationSources        => _data.modulationSources.AsSpan();
    public static ReadOnlySpan<string> ModulationDestinations   => _data.modulationDestinations.AsSpan();
    public static ReadOnlySpan<string> FilterTypes              => _data.filterTypes.AsSpan();
    public static ReadOnlySpan<string> ScaleTypes               => _data.scaleTypes.AsSpan();
    public static ReadOnlySpan<string> MfxTypes                 => _data.mfxTypes.AsSpan();
    public static ReadOnlySpan<string> IfxTypes                 => _data.ifxTypes.AsSpan();
    public static ReadOnlySpan<string> GrooveTypes              => _data.grooveTypes.AsSpan();

    public static void Initialize(in StringTable source) => _data = source;

    static StringTable _data;
}
