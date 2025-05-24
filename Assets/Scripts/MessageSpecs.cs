using System;
using System.Runtime.InteropServices;

namespace MessageSpecs {

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct Pattern
{
    public fixed byte header[4];            // 0–3: 'PTST'
    public uint size;                       // 4–7
    public fixed byte reserved0[8];         // 8–15

    public fixed byte patternName[18];      // 16–33: null-terminated
    public ushort tempo;                    // 34–35: 200~3000 = 20.0~300.0
    public sbyte swing;                     // 36: -48~48
    public byte length;                     // 37: 0~3 = 1~4 bar
    public byte beat;                       // 38: 0~3 = 16, 32, 8 tri, 16 tri
    public byte key;                        // 39: 0~11 = C~B
    public byte scale;                      // 40: 0~35
    public byte chordSet;                   // 41: 0~4
    public byte playLevel;                  // 42: 127~0 = 0~127
    public byte reserved1;                  // 43

    public fixed byte touchScaleParam[16];  // 42~57
    public fixed byte masterFxParam[8];     // 58~65
    public byte alt13_14;                   // 66: 0~1=OFF,ON
    public byte alt15_16;                   // 67: 0~1=OFF,ON
    public fixed byte reserved2[8];         // 68~77
    public fixed byte reserved3[178];       // 78~255

    public fixed byte motionSequence[1584]; // 256~1839 (Table 5)
    public fixed byte reserved4[208];       // 1840~2048

    public fixed byte part1[816];           // 2048~2863 (PartParameter x1)
    public fixed byte part2[816];
    public fixed byte part3[816];
    public fixed byte part4[816];
    public fixed byte part5[816];
    public fixed byte part6[816];
    public fixed byte part7[816];
    public fixed byte part8[816];
    public fixed byte part9[816];
    public fixed byte part10[816];
    public fixed byte part11[816];
    public fixed byte part12[816];
    public fixed byte part13[816];
    public fixed byte part14[816];
    public fixed byte part15[816];
    public fixed byte part16[816];          // 14288~15103

    public fixed byte reserved5[252];       // 15104~15355
    public fixed byte footer[4];            // 15356~15359: 'PTED'
    public fixed byte reserved6[1024];      // 15360~16383
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct Part
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

    public fixed byte steps[768];       // 48~815
}

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

} // namespace MessageSpecs
