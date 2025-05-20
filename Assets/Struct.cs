using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct StepData
{
    public byte onOff;                  // 0
    public byte gateTime;               // 1
    public byte velocity;               // 2
    public byte trigger;                // 3

    public byte noteSlot1;              // 4
    public byte noteSlot2;              // 5
    public byte noteSlot3;              // 6
    public byte noteSlot4;              // 7

    public fixed byte reserved[4];      // 8-11
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct PartParameter
{
    public byte lastStep;               // 0
    public byte mute;                   // 1
    public byte voiceAssign;            // 2
    public byte motionSequence;         // 3
    public byte trigPadVelocity;        // 4
    public byte scaleMode;              // 5
    public byte partPriority;           // 6
    public byte reserved0;              // 7

    public ushort oscillatorType;       // 8~9
    public byte reserved1;              // 10

    public byte oscillatorEdit;         // 11
    public byte filterType;             // 12
    public byte filterCutoff;           // 13
    public byte filterResonance;        // 14
    public sbyte filterEgInt;           // 15

    public byte modulationType;         // 16
    public byte modulationSpeed;        // 17
    public byte modulationDepth;        // 18
    public byte reserved2;              // 19

    public byte egAttack;               // 20
    public byte egDecayRelease;         // 21
    public ushort reserved3;            // 22~23

    public byte ampLevel;               // 24
    public sbyte ampPan;                // 25
    public byte egOnOff;                // 26
    public byte mfxSendOnOff;           // 27
    public byte grooveType;             // 28
    public byte grooveDepth;            // 29
    public ushort reserved4;            // 30~31

    public byte ifxOnOff;               // 32
    public byte ifxType;                // 33
    public byte ifxEdit;                // 34
    public byte reserved5;              // 35

    public sbyte oscillatorPitch;       // 36
    public byte oscillatorGlide;        // 37

    public fixed byte reserved6[10];    // 38~47

    public fixed byte steps[768];       // 48~(48+64×12=816)
}
