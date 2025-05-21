using System;

sealed class MessageSender : IDisposable
{
    RtMidi.MidiOut _midiPort;

    static readonly byte[] CurrentPatternDataDumpRequest =
      { 0xF0, 0x42, 0x30, 0x00, 0x01, 0x23, 0x10, 0xF7 };

    public MessageSender()
      => _midiPort = MidiPortProbe.OpenOutPort();

    public void Dispose()
    {
        _midiPort?.Dispose();
        _midiPort = null;
    }

    public void SendCurrentPatternDataDumpRequest()
      => _midiPort.SendMessage(CurrentPatternDataDumpRequest);
}
