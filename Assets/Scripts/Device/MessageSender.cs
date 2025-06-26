using System;
using System.Diagnostics;
using System.Threading;

public sealed class MessageSender : IDisposable
{
    #region Public properties

    public float ClockTempo { get; set; }

    #endregion

    #region Clock control

    public bool IsPlaying
      => _playEvent.IsSet;

    public void StartPlaying()
    {
        SendSingleByte(0xFA);
        _playEvent.Set();
    }

    public void StopPlaying()
    {
        SendSingleByte(0xFC);
        SendSingleByte(0xFC);
        _playEvent.Reset();
    }

    #endregion

    #region MIDI message sender

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

    public void SendNoteOn(int channel, int note, int velocity)
    {
        _buffer[0] = (byte)(0x90 + channel);
        _buffer[1] = (byte)note;
        _buffer[2] = (byte)velocity;
        _midiPort.SendMessage(_buffer.AsSpan(0, 3));
    }

    public void SendNoteOff(int channel, int note)
    {
        _buffer[0] = (byte)(0x80 + channel);
        _buffer[1] = (byte)note;
        _buffer[2] = 0;
        _midiPort.SendMessage(_buffer.AsSpan(0, 3));
    }

    public void SendSingleByte(byte data)
    {
        _buffer[0] = data;
        _midiPort.SendMessage(_buffer.AsSpan(0, 1));
    }

    #endregion

    #region Private members

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

    #endregion

    #region Clock sender thread

    Thread _clockThread;
    ManualResetEventSlim _playEvent;
    bool _running;

    double ClockInterval
      => 60.0 / (24 * Math.Max(80, ClockTempo));

    void StartClockThread()
    {
        // Initialization
        _clockThread = new Thread(ClockThreadMethod) { IsBackground = true };
        _playEvent = new ManualResetEventSlim(false);
        _running = true;

        // Thread start
        _clockThread.Start();
    }

    void StopClockThread()
    {
        // Stop state
        _running = false;
        _playEvent.Set();

        // Thread finalization
        _clockThread.Join();
        _clockThread = null;

        // Play event finalization
        _playEvent.Dispose();
        _playEvent = null;
    }

    void ClockThreadMethod()
    {
        var sw = Stopwatch.StartNew();

        while (_running)
        {
            // Wait for play event.
            _playEvent.Wait();

            // Next tick: The fist tick is sent immediately.
            var nextTick = sw.ElapsedTicks;

            // Repeat until the stop condition is met.
            while (_running && _playEvent.IsSet)
            {
                if (sw.ElapsedTicks < nextTick)
                {
                    // Before next tick: Do spin wait.
                    Thread.SpinWait(1000);
                }
                else
                {                    // Next tick reached: Send clock and update next tick.
                    lock (this) SendSingleByte(0xF8);
                    nextTick += (long)(ClockInterval * Stopwatch.Frequency);
                }
            }
        }
    }

    #endregion

    #region Lifecycle

    public MessageSender()
    {
        _midiPort = MidiPortProbe.OpenOutPort();
        StartClockThread();
    }

    public void Dispose()
    {
        StopClockThread();
        _midiPort?.Dispose();
        _midiPort = null;
    }

    #endregion
}
