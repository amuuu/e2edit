using System;
using System.Runtime.InteropServices;

sealed class MessageReceiver : IDisposable
{
    public ReadOnlySpan<byte> PatternBuffer
      => new ReadOnlySpan<byte>(_buffer, 0, 16384);

    public int PatternUpdateCount => _patternUpdateCount;

    RtMidi.MidiIn _midiPort;
    byte[] _buffer = new byte[32 * 1024];
    int _patternUpdateCount;

    public MessageReceiver()
    {
        _midiPort = MidiPortProbe.OpenInPort();
        _midiPort.MessageReceived = OnMessageReceived;
    }

    public void Dispose()
    {
        _midiPort?.Dispose();
        _midiPort = null;
    }

    void OnMessageReceived(double time, ReadOnlySpan<byte> data)
    {
        if (data[0] != 0xf0) return; // SysEx message type
        if (data[1] != 0x42) return; // Manufacturers ID
        if (data[3] != 0x00) return; // Product ID
        if (data[4] != 0x01) return;
        if (data[5] != 0x23) return;
        if (data[6] != 0x40) return; // Current Pattern Data Dump
        SysExCodec.DecodeTo8Bit(data.Slice(7, 18720), _buffer);
        _patternUpdateCount++;
    }
}
