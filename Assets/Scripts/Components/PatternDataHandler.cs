using System;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class PatternDataHandler : MonoBehaviour
{
    #region Singleton-like pulic interface

    public static ref PatternDataView Data => ref _data;

    public static async Awaitable ReceiveFromDeviceAsync()
    {
        try
        {
            var count = _receiver.PatternUpdateCount;
            _sender.SendCurrentPatternDataDumpRequest();
            while (count == _receiver.PatternUpdateCount)
                await Awaitable.NextFrameAsync();
            _receiver.PatternBuffer.CopyTo(_data.AsBytes);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static async Awaitable SendToDeviceAsync()
    {
        _sender.SendPatternData(_data.AsBytes);
        await Awaitable.NextFrameAsync();
        // TODO: Check response
    }

    public static async Awaitable PlayCurrentStepAsync(float duration)
    {
        var channel = _data.PartSelect - 1;
        var note1 = _data.StepNote1 - 1;
        var note2 = _data.StepNote2 - 1;
        var note3 = _data.StepNote3 - 1;
        var note4 = _data.StepNote4 - 1;
        var vel = _data.StepVelocity;

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

    static PatternDataView _data;
    static MessageReceiver _receiver;
    static MessageSender _sender;

    #endregion

    #region MonoBehaviour implementation

    void OnEnable()
    {
        if (_data != null) return;

        _data = new PatternDataView();
        _receiver = new MessageReceiver();
        _sender = new MessageSender();

        GetComponent<UIDocument>().rootVisualElement.dataSource = _data;
    }

    void OnDisable()
    {
        _sender?.Dispose();
        _receiver?.Dispose();
        (_data, _sender, _receiver) = (null, null, null);
    }

    #endregion
}
