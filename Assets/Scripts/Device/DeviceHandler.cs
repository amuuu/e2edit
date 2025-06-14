using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class DeviceHandler
{
    #region Public properties

    public static float ClockTempo { get; set; }

    #endregion

    #region Lifecycle

    public static void SetUp()
      => StartClockThread();

    public static void TearDown()
      => StopClockThread();

    #endregion

    #region Data transfer

    public static async Awaitable ReceivePatternAsync(PatternDataView dest)
    {
        try
        {
            // Send request and wait for receiver to update.
            var count = _receiver.PatternUpdateCount;
            lock (_sender) _sender.SendCurrentPatternDataDumpRequest();
            while (count == _receiver.PatternUpdateCount)
                await Awaitable.NextFrameAsync();

            // Pattern data update
            _receiver.PatternBuffer.CopyTo(dest.AsBytes);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static async Awaitable SendPatternAsync(PatternDataView src)
    {
        var count = _receiver.AckCount;
        lock (_sender) _sender.SendPatternData(src.AsBytes);
        while (count == _receiver.AckCount) await Awaitable.NextFrameAsync();
    }

    #endregion

    #region Playback

    public static bool IsPlaying
      => _playEvent.IsSet;

    public static void StartPlaying()
    {
        _sender.SendSingleByte(0xFA);
        _playEvent.Set();
    }

    public static void StopPlaying()
    {
        _sender.SendSingleByte(0xFC);
        _sender.SendSingleByte(0xFC);
        _playEvent.Reset();
    }

    public static async Awaitable
      PlayCurrentStepAsync(PatternDataView data, float duration)
    {
        var channel = data.PartSelect - 1;
        var note1 = data.StepNote1 - 1;
        var note2 = data.StepNote2 - 1;
        var note3 = data.StepNote3 - 1;
        var note4 = data.StepNote4 - 1;
        var vel = data.StepVelocity;

        lock (_sender)
        {
            if (note1 >= 0) _sender.SendNoteOn(channel, note1, vel);
            if (note2 >= 0) _sender.SendNoteOn(channel, note2, vel);
            if (note3 >= 0) _sender.SendNoteOn(channel, note3, vel);
            if (note4 >= 0) _sender.SendNoteOn(channel, note4, vel);
        }

        await Awaitable.WaitForSecondsAsync(duration);

        lock (_sender)
        {
            if (note1 >= 0) _sender.SendNoteOff(channel, note1);
            if (note2 >= 0) _sender.SendNoteOff(channel, note2);
            if (note3 >= 0) _sender.SendNoteOff(channel, note3);
            if (note4 >= 0) _sender.SendNoteOff(channel, note4);
        }
    }

    #endregion

    #region Message sender/receiver

    static MessageReceiver _receiver = new MessageReceiver();
    static MessageSender _sender = new MessageSender();

    #endregion

    #region Clock sender thread

    static Thread _clockThread;
    static ManualResetEventSlim _playEvent;
    static bool _running;

    static double ClockInterval
      => 60.0 / (24 * Math.Max(80, ClockTempo));

    static void StartClockThread()
    {
        // Initialization
        _clockThread = new Thread(ClockThreadMethod) { IsBackground = true };
        _playEvent = new ManualResetEventSlim(false);
        _running = true;

        // Thread start
        _clockThread.Start();
    }

    static void StopClockThread()
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

    static void ClockThreadMethod()
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
                {
                    // Next tick reached: Send clock and update next tick.
                    lock (_sender) _sender.SendSingleByte(0xF8);
                    nextTick += (long)(ClockInterval * Stopwatch.Frequency);
                }
            }
        }
    }

    #endregion

}
