using System;
using UnityEngine;

public static class PatternDataHandler
{
    #region Public interface

    public static ref PatternDataView Data => ref _data;

    public static event Action DataRefreshed;

    public static async Awaitable ReceiveFromDeviceAsync()
    {
        try
        {
            var count = DeviceHandler.Receiver.PatternUpdateCount;

            DeviceHandler.Sender.SendCurrentPatternDataDumpRequest();

            while (count == DeviceHandler.Receiver.PatternUpdateCount)
                await Awaitable.NextFrameAsync();

            DeviceHandler.Receiver.PatternBuffer.CopyTo(_data.AsBytes);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        DataRefreshed.Invoke();
    }

    public static async Awaitable SendToDeviceAsync()
    {
        DeviceHandler.Sender.SendPatternData(_data.AsBytes);

        // TODO: Check response
        await Awaitable.NextFrameAsync();
    }

    public static async Awaitable PlayCurrentStepAsync(float duration)
    {
        var sender = DeviceHandler.Sender;

        var channel = _data.PartSelect - 1;
        var note1 = _data.StepNote1 - 1;
        var note2 = _data.StepNote2 - 1;
        var note3 = _data.StepNote3 - 1;
        var note4 = _data.StepNote4 - 1;
        var vel = _data.StepVelocity;

        if (note1 >= 0) sender.SendNoteOn(channel, note1, vel);
        if (note2 >= 0) sender.SendNoteOn(channel, note2, vel);
        if (note3 >= 0) sender.SendNoteOn(channel, note3, vel);
        if (note4 >= 0) sender.SendNoteOn(channel, note4, vel);

        await Awaitable.WaitForSecondsAsync(duration);

        if (note1 >= 0) sender.SendNoteOff(channel, note1);
        if (note2 >= 0) sender.SendNoteOff(channel, note2);
        if (note3 >= 0) sender.SendNoteOff(channel, note3);
        if (note4 >= 0) sender.SendNoteOff(channel, note4);
    }

    #endregion

    #region Private objects

    static PatternDataView _data = new PatternDataView();

    #endregion
}
