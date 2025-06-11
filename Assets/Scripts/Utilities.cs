using UnityEngine;
using UnityEngine.UIElements;
using Math = System.Math;

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
          { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        for (var i = 0; i < 128; i++)
        {
            var name = names[i % 12];
            var octave = i / 12 - 1;
            _noteNames[i + 1] = $"{name}{octave}";
        }
    }

    public static string GetNoteName(int note) => _noteNames[note];

    public static void Transpose(ref MessageSpecs.Step step, int delta)
    {
        if (step.noteSlot1 > 0) step.noteSlot1 = (byte)Math.Clamp(step.noteSlot1 + delta, 1, 128);
        if (step.noteSlot2 > 0) step.noteSlot2 = (byte)Math.Clamp(step.noteSlot2 + delta, 1, 128);
        if (step.noteSlot3 > 0) step.noteSlot3 = (byte)Math.Clamp(step.noteSlot3 + delta, 1, 128);
        if (step.noteSlot4 > 0) step.noteSlot4 = (byte)Math.Clamp(step.noteSlot4 + delta, 1, 128);
    }
}
