using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class DeviceHandler
{
    public static float ClockTempo { get; set; } = 120;

    static Thread _clockThread;
    static ManualResetEventSlim _playEvent;
    static bool _running;

    public static void StartClockThread()
    {
        _clockThread = new Thread(SendClockLoop) { IsBackground = true };
        _playEvent = new ManualResetEventSlim(false);
        _running = true;

        _clockThread.Start();
    }

    public static void StopClockThread()
    {
        _running = false;
        _playEvent.Set();
        _clockThread?.Join();
        _clockThread = null;
    }

    static void SendClockLoop()
    {
        var stopwatch = Stopwatch.StartNew();

        while (_running)
        {
            _playEvent.Wait();

            var nextTick = stopwatch.ElapsedTicks;

            while (_running && _playEvent.IsSet)
            {
                while (_running && _playEvent.IsSet && stopwatch.ElapsedTicks < nextTick)
                    Thread.SpinWait(1000);

                if (!_running || !_playEvent.IsSet) break;

                lock (_sender) _sender.SendSingleByte(0xF8);

                var interval = 60.0 / (24 * Math.Max(80, ClockTempo));
                nextTick += (long)(interval * Stopwatch.Frequency);
            }
        }
    }

    #region Data transfer

    public static async Awaitable ReceivePatternAsync(PatternDataView dest)
    {
        try
        {
            var count = _receiver.PatternUpdateCount;

            lock (_sender) _sender.SendCurrentPatternDataDumpRequest();

            while (count == _receiver.PatternUpdateCount)
                await Awaitable.NextFrameAsync();

            _receiver.PatternBuffer.CopyTo(dest.AsBytes);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static async Awaitable SendPatternAsync(PatternDataView src)
    {
        lock (_sender) _sender.SendPatternData(src.AsBytes);

        // TODO: Check response
        await Awaitable.NextFrameAsync();
    }

    #endregion

    #region Playback

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

    #region Private objects

    static MessageReceiver _receiver = new MessageReceiver();
    static MessageSender _sender = new MessageSender();

    #endregion
}
