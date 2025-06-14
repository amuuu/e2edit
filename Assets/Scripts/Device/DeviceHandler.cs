using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class DeviceHandler
{
    public static float ClockTempo { get; set; } = 120;

    static Thread _thread;
    static bool _running;

    public static void StartClockThread()
    {
        _running = true;
        _thread = new Thread(SendClockLoop);
        _thread.IsBackground = true;
        _thread.Start();
    }

    public static void StopClockThread()
    {
        _running = false;
        _thread?.Join();
        _thread = null;
    }

    static void SendClockLoop()
    {
        var stopwatch = Stopwatch.StartNew();
        var nextTick = stopwatch.ElapsedTicks;

        while (_running)
        {
            var now = stopwatch.ElapsedTicks;

            var tempo = System.Math.Max(80, ClockTempo);
            var intervalMs = (60000.0 / tempo) / 24.0;
            var intervalTicks = intervalMs * (Stopwatch.Frequency / 1000.0);

            if (now >= nextTick)
            {
                lock (_sender) _sender.SendSingleByte(0xF8);
                nextTick += (long)intervalTicks;
            }
            else
            {
                Thread.SpinWait(1000);
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
      => _sender.SendSingleByte(0xFA);

    public static void StopPlaying()
      => _sender.SendSingleByte(0xFB);

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
