using System;
using System.Runtime.InteropServices;
using UnityEngine;

public sealed class IOManager : IDisposable
{
    #region Buffer accessors

    public ReadOnlySpan<PartParameter> PartsInPattern
      => MemoryMarshal.Cast<byte, PartParameter>
           (_patternBuffer.Buffer.Slice(2048, 816 * 16));

    #endregion

    #region Public methods

    public void Dispose()
    {
        _inPort?.Dispose();
        _outPort?.Dispose();
        (_inPort, _outPort) = (null, null);
    }

    public bool TryOpenPorts()
    {
        _inPort = SearchAndOpenInPort();
        _outPort = SearchAndOpenOutPort();

        if (_inPort == null || _outPort == null)
        {
            Debug.Log("Electribe device not found.");
            return false;
        }

        Debug.Log("Electribe device found.");
        return true;
    }

    public async Awaitable<bool> RequestCurrentPatternDump()
    {
        var count = _patternCount;
        _outPort.SendMessage(Request.CurrentPatternDataDump);
        while (_patternCount == count) await Awaitable.NextFrameAsync();
        return true;
    }

    #endregion

    #region I/O port management

    RtMidi.MidiIn _inPort;
    RtMidi.MidiOut _outPort;

    RtMidi.MidiIn SearchAndOpenInPort()
    {
        var port = RtMidi.MidiIn.Create();
        for (var i = 0; i < port.PortCount; i++)
        {
            if (port.GetPortName(i).StartsWith("electribe2"))
            {
                port.OpenPort(i);
                port.MessageReceived = OnMessageReceived;
                port.IgnoreTypes(sysex: false);
                return port;
            }
        }
        port.Dispose();
        return null;
    }

    RtMidi.MidiOut SearchAndOpenOutPort()
    {
        var port = RtMidi.MidiOut.Create();
        for (var i = 0; i < port.PortCount; i++)
        {
            if (port.GetPortName(i).StartsWith("electribe2"))
            {
                port.OpenPort(i);
                return port;
            }
        }
        port.Dispose();
        return null;
    }

    #endregion

    #region MIDI event handler

    SysExDataBuffer _patternBuffer = SysExDataBuffer.Create7Bit(18720);
    int _patternCount;

    void OnMessageReceived(double time, ReadOnlySpan<byte> data)
    {
        if (data[0] != 0xf0) return; // SysEx message type
        if (data[1] != 0x42) return; // Manufacturers ID
        if (data[3] != 0x00) return; // Product ID
        if (data[4] != 0x01) return;
        if (data[5] != 0x23) return;
        if (data[6] != 0x40) return; // Current Pattern Data Dump
        data.Slice(7, 18720).CopyTo(_patternBuffer.Buffer);
        _patternBuffer.DecodeTo8Bit();
        _patternCount++;
    }

    #endregion
}
