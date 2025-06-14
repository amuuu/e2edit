using System;
using System.Runtime.InteropServices;

public sealed class MessageReceiver : IDisposable
{
    #region Public accessors

    public ReadOnlySpan<byte> PatternBuffer
      => new ReadOnlySpan<byte>(_buffer, 0, 16384);

    public int PatternUpdateCount => _patternUpdateCount;

    public int AckCount => _ackCount;

    #endregion

    #region Lifecycle

    public MessageReceiver()
    {
        _midiPort = MidiPortProbe.OpenInPort();
        if (_midiPort == null) return;
        _midiPort.MessageReceived = OnMessageReceived;
    }

    public void Dispose()
    {
        _midiPort?.Dispose();
        _midiPort = null;
    }

    #endregion

    #region Private members

    RtMidi.MidiIn _midiPort;
    byte[] _buffer = new byte[32 * 1024];
    int _patternUpdateCount, _ackCount;

    void OnMessageReceived(double time, ReadOnlySpan<byte> data)
    {
        if (data[0] != 0xf0) return; // SysEx message type
        if (data[1] != 0x42) return; // Manufacturers ID
        if (data[3] != 0x00) return; // Product ID
        if (data[4] != 0x01) return;
        if (data[5] != 0x23) return;

        // Ack/Nak
        if (data[6] == 0x23 || data[6] == 0x24) _ackCount++;

        // Current Pattern Data Dump
        if (data[6] == 0x40)
        {
            var body_len = data.Length - 7 - 1;
            var sp = SysExCodec.DecodeTo8Bit(data.Slice(7, body_len), _buffer);
            _patternUpdateCount++;
        }
    }

    #endregion
}
