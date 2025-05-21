using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;

public sealed class SysExTest : MonoBehaviour
{
    MessageSender _sender;
    MessageReceiver _receiver;

    async Awaitable Start()
    {
        _sender = new MessageSender();
        _receiver = new MessageReceiver();

        var count = _receiver.PatternCount;
        _sender.SendCurrentPatternDataDumpRequest();
        while (count == _receiver.PatternCount) await Awaitable.NextFrameAsync();
        DumpPatternData();
    }

    void DumpPatternData()
    {
        foreach (var part in _receiver.PartsInPattern)
            Debug.Log(part.ampLevel);
    }

    void OnDestroy()
    {
        _sender?.Dispose();
        _receiver?.Dispose();
        (_sender, _receiver) = (null, null);
    }
}
