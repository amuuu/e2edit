using UnityEngine;

public static class AsyncUtil
{
    public static void Forget(Awaitable awaitable) {}
}

public static class NoteUtil
{
    static string[] _noteNames;

    static NoteUtil()
    {
        _noteNames = new string[129];
        _noteNames[0] = "Off";

        var names = new[]
          { "C", "C#", "D", "D#", "E", "F",
            "F#", "G", "G#", "A", "A#", "B" };

        for (var i = 0; i < 128; i++)
        {
            var name = names[i % 12];
            var octave = i / 12 - 1;
            _noteNames[i + 1] = $"{name}{octave}";
        }
    }

    public static string GetNoteName(int note) => _noteNames[note];
}
