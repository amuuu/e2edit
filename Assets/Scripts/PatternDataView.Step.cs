using Unity.Properties;
using System.Runtime.InteropServices;

public sealed partial class PatternDataView
{
    #region Step data window

    [CreateProperty]
    public int StepSelect { get; set; } = 1;

    public ref MessageSpecs.Step CurrentStep
      => ref GetStepRef(PartSelect - 1, StepSelect - 1);

    public ref MessageSpecs.Step GetStepRef(int part, int step)
    {
        var source = MemoryMarshal.CreateSpan(ref Data, 1);
        var offs = 2048 + 816 * part + 48 + 12 * step;
        var span = MemoryMarshal.AsBytes(source).Slice(offs, 12);
        return ref MemoryMarshal.Cast<byte, MessageSpecs.Step>(span)[0];
    }

    #endregion

    #region Current step data accessors

    [CreateProperty]
    public bool StepOnOff
      { get => CurrentStep.onOff > 0;
        set => CurrentStep.onOff = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public int StepGateTime
      { get => System.Math.Min(97, (int)CurrentStep.gateTime);
        set => CurrentStep.gateTime = (byte)(value == 97 ? 127 : value); }

    [CreateProperty]
    public string StepGateTimeText
      => StepGateTime == 97 ? "Tie" : $"{StepGateTime * 100 / 97}%";

    [CreateProperty]
    public int StepVelocity
      { get => CurrentStep.velocity;
        set => CurrentStep.velocity = (byte)value; }

    [CreateProperty]
    public bool StepTrigger
      { get => CurrentStep.trigger > 0;
        set => CurrentStep.trigger = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public int StepNote1
      { get => CurrentStep.noteSlot1;
        set => CurrentStep.noteSlot1 = (byte)value; }

    [CreateProperty]
    public int StepNote2
      { get => CurrentStep.noteSlot2;
        set => CurrentStep.noteSlot2 = (byte)value; }

    [CreateProperty]
    public int StepNote3
      { get => CurrentStep.noteSlot3;
        set => CurrentStep.noteSlot3 = (byte)value; }

    [CreateProperty]
    public int StepNote4
      { get => CurrentStep.noteSlot4;
        set => CurrentStep.noteSlot4 = (byte)value; }

    [CreateProperty]
    public string StepNote1Name
      => NoteUtil.GetNoteName(StepNote1);

    [CreateProperty]
    public string StepNote2Name
      => NoteUtil.GetNoteName(StepNote2);

    [CreateProperty]
    public string StepNote3Name
      => NoteUtil.GetNoteName(StepNote3);

    [CreateProperty]
    public string StepNote4Name
      => NoteUtil.GetNoteName(StepNote4);

    #endregion
}
