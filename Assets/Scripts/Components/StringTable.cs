using UnityEngine;

public sealed class StringTable : ScriptableObject
{
    public string[] oscillatorTypes = null;
    public string[] oscillatorCategories = null;
    public string[] modulationTypes = null;
    public string[] modulationSources = null;
    public string[] modulationDestinations = null;
    public string[] filterTypes = null;
    public string[] scaleTypes = null;
    public string[] mfxTypes = null;
    public string[] ifxTypes = null;
    public string[] grooveTypes = null;

    static public StringTable Instance { get; private set; }

    void OnEnable() => Instance = this;
}
