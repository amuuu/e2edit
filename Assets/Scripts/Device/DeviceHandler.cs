using System;
using UnityEngine;

public static class DeviceHandler
{
    #region Async communication

    public static async Awaitable ReceivePatternAsync(PatternDataView dest)
    {
        try
        {
            var count = _receiver.PatternUpdateCount;

            _sender.SendCurrentPatternDataDumpRequest();

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
        _sender.SendPatternData(src.AsBytes);

        // TODO: Check response
        await Awaitable.NextFrameAsync();
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

        if (note1 >= 0) _sender.SendNoteOn(channel, note1, vel);
        if (note2 >= 0) _sender.SendNoteOn(channel, note2, vel);
        if (note3 >= 0) _sender.SendNoteOn(channel, note3, vel);
        if (note4 >= 0) _sender.SendNoteOn(channel, note4, vel);

        await Awaitable.WaitForSecondsAsync(duration);

        if (note1 >= 0) _sender.SendNoteOff(channel, note1);
        if (note2 >= 0) _sender.SendNoteOff(channel, note2);
        if (note3 >= 0) _sender.SendNoteOff(channel, note3);
        if (note4 >= 0) _sender.SendNoteOff(channel, note4);
    }

    #endregion

    #region Private objects

    static MessageReceiver _receiver = new MessageReceiver();
    static MessageSender _sender = new MessageSender();

    #endregion
}
