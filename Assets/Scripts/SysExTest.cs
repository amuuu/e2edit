using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class SysExTest : MonoBehaviour
{
    MessageSender _sender;
    MessageReceiver _receiver;
    PatternDataView _pattern = new();

    async Awaitable Start()
    {
        GetComponent<UIDocument>().rootVisualElement.dataSource = _pattern;

        try
        {
            _sender = new MessageSender();
            _receiver = new MessageReceiver();

            var count = _receiver.PatternUpdateCount;
            _sender.SendCurrentPatternDataDumpRequest();
            while (count == _receiver.PatternUpdateCount)
                await Awaitable.NextFrameAsync();

            _pattern.UpdateData(_receiver.PatternBuffer);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    void OnDestroy()
    {
        _sender?.Dispose();
        _receiver?.Dispose();
        (_sender, _receiver) = (null, null);
    }
}
