using System;
using UnityEngine;

public static class DeviceHandler
{
    #region Public properties

    public static float ClockTempo
      { get => _sender.ClockTempo; set => _sender.ClockTempo = value; }

    #endregion

    #region Lifecycle

    public static void SetUp()
    {
        _sender = new MessageSender();
        _receiver = new MessageReceiver();
    }

    public static void TearDown()
    {
        _sender.Dispose();
        _sender = null;

        _receiver.Dispose();
        _receiver = null;
    }

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
      => _sender.IsPlaying;

    public static void StartPlaying()
      => _sender.StartPlaying();

    public static void StopPlaying()
      => _sender.StopPlaying();

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

    static MessageSender _sender;
    static MessageReceiver _receiver;

    #endregion
}
