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
