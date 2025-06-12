using System;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class PatternDataHandler : MonoBehaviour
{
    #region Public interface

    public static ref PatternDataView Data => ref _data;

    public async Awaitable ReceiveFromDeviceAsync()
    {
        var device = GetComponent<DeviceHandler>();

        try
        {
            var count = device.Receiver.PatternUpdateCount;

            device.Sender.SendCurrentPatternDataDumpRequest();

            while (count == device.Receiver.PatternUpdateCount)
                await Awaitable.NextFrameAsync();

            device.Receiver.PatternBuffer.CopyTo(_data.AsBytes);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async Awaitable SendToDeviceAsync()
    {
        var device = GetComponent<DeviceHandler>();

        device.Sender.SendPatternData(_data.AsBytes);

        // TODO: Check response
        await Awaitable.NextFrameAsync();
    }

    public async Awaitable PlayCurrentStepAsync(float duration)
    {
        var device = GetComponent<DeviceHandler>();

        var channel = _data.PartSelect - 1;
        var note1 = _data.StepNote1 - 1;
        var note2 = _data.StepNote2 - 1;
        var note3 = _data.StepNote3 - 1;
        var note4 = _data.StepNote4 - 1;
        var vel = _data.StepVelocity;

        if (note1 >= 0) device.Sender.SendNoteOn(channel, note1, vel);
        if (note2 >= 0) device.Sender.SendNoteOn(channel, note2, vel);
        if (note3 >= 0) device.Sender.SendNoteOn(channel, note3, vel);
        if (note4 >= 0) device.Sender.SendNoteOn(channel, note4, vel);

        await Awaitable.WaitForSecondsAsync(duration);

        if (note1 >= 0) device.Sender.SendNoteOff(channel, note1);
        if (note2 >= 0) device.Sender.SendNoteOff(channel, note2);
        if (note3 >= 0) device.Sender.SendNoteOff(channel, note3);
        if (note4 >= 0) device.Sender.SendNoteOff(channel, note4);
    }

    #endregion

    #region Private objects

    static PatternDataView _data = new PatternDataView();

    #endregion

    #region MonoBehaviour implementation

    void OnEnable()
      => GetComponent<UIDocument>().rootVisualElement.dataSource = _data;

    #endregion
}
