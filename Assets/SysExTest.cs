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
        unsafe {Debug.Log(sizeof(Pattern));}
        try
        {
            _sender = new MessageSender();
            _receiver = new MessageReceiver();

            var count = _receiver.PatternUpdateCount;
            _sender.SendCurrentPatternDataDumpRequest();
            while (count == _receiver.PatternUpdateCount)
                await Awaitable.NextFrameAsync();
            DumpPatternData();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    unsafe void DumpPatternData()
    {
        var pattern = _receiver.PatternBuffer;
        fixed (byte* name = pattern[0].patternName) {
        Debug.Log(MessageUtil.GetString(new Span<byte>(name, 18)));
        }

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
