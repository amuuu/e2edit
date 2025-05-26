using Unity.Properties;

public sealed partial class PatternDataView
{
    #region Step data accessors

    [CreateProperty]
    public int StepSelect { get; set; } = 1;

    [CreateProperty]
    public bool StepOnOff
      { get => CurrentStep.onOff > 0;
        set => CurrentStep.onOff = (byte)(value ? 1 : 0); }

    [CreateProperty]
    public int StepGateTime
      { get => CurrentStep.gateTime;
        set => CurrentStep.gateTime = (byte)value; }

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

    #endregion
}
