using UnityEngine;

[System.Serializable]
public struct OscillatorType
{
    public enum Category
    {
        Kick, Snare, Clip, HiHat, Cymbal, Tom, Percussion, Voice,
        SynthFx, SynthHit, InstHit, Synth, Instrument, AudioIn
    }

    public string name;
    public Category category;
}

public sealed class OscillatorTypeCatalog : ScriptableObject
{
    [SerializeField] OscillatorType[] _typeList = null;

    static public OscillatorTypeCatalog Instance { get; private set; }

    public OscillatorType this[int index] => _typeList[index];

    void OnEnable() => Instance = this;
}
