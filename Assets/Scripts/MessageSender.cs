using System;

sealed class MessageSender : IDisposable
{
    RtMidi.MidiOut _midiPort;

    byte[] CurrentPatternDataDumpRequest =
      { 0xF0, 0x42, 0x30, 0x00, 0x01, 0x23, 0x10, 0xF7 };

    byte[] PatternDataDumpRequest =
      { 0xF0, 0x42, 0x30, 0x00, 0x01, 0x23, 0x1c, 0, 0, 0xF7 };

    byte[] GlobalDataDumpRequest =
      { 0xF0, 0x42, 0x30, 0x00, 0x01, 0x23, 0x1e, 0xF7 };

    public MessageSender()
      => _midiPort = MidiPortProbe.OpenOutPort();

    public void Dispose()
    {
        _midiPort?.Dispose();
        _midiPort = null;
    }

    public void SendCurrentPatternDataDumpRequest()
      => _midiPort.SendMessage(CurrentPatternDataDumpRequest);

    public void SendPatternDataDumpRequest(int pattern)
    {
        PatternDataDumpRequest[7] = (byte)((pattern     ) & 0x7f);
        PatternDataDumpRequest[8] = (byte)((pattern >> 7) & 0x7f);
        _midiPort.SendMessage(PatternDataDumpRequest);
    }

    public void SendGlobalDataDumpRequest()
      => _midiPort.SendMessage(GlobalDataDumpRequest);
}
