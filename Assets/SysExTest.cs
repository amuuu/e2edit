using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;

public sealed class SysExTest : MonoBehaviour
{
    MessageSender _sender;
    MessageReceiver _receiver;
    PatternDataView _pattern = new();

    async Awaitable Start()
    {
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
        _pattern.UpdateData(_receiver.PatternBuffer);
        Debug.Log(_pattern.PatternName);
    }

    void OnDestroy()
    {
        _sender?.Dispose();
        _receiver?.Dispose();
        (_sender, _receiver) = (null, null);
    }
}
