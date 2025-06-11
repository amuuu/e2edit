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

    byte[] PatternDataDumpHeader =
      { 0xF0, 0x42, 0x30, 0x00, 0x01, 0x23, 0x40 };

    byte[] _buffer = new byte[32 * 1024];

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

    public void SendPatternData(ReadOnlySpan<byte> data)
    {
        var header = PatternDataDumpHeader.AsSpan();
        header.CopyTo(_buffer);
        var body = SysExCodec.EncodeTo7Bit(data, _buffer.AsSpan(header.Length));
        _buffer[header.Length + body.Length] = 0xF7;
        _midiPort.SendMessage(_buffer.AsSpan(0, header.Length + body.Length + 1));
    }
}
