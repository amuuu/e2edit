using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;

public sealed class SysExTest : MonoBehaviour
{
    IOManager _io;

    async Awaitable Start()
    {
        _io = new IOManager();
        if (!_io.TryOpenPorts()) return;
        if (await _io.RequestCurrentPatternDump()) DumpPatternData();
    }

    void DumpPatternData()
    {
        foreach (var part in _io.PartsInPattern)
            Debug.Log(part.ampLevel);
    }

    void OnDestroy()
    {
        _io?.Dispose();
        _io = null;
    }
}
