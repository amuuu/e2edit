using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class PatternEditor : MonoBehaviour
{
    MessageSender _sender;
    MessageReceiver _receiver;
    PatternDataView _pattern = new();

    void Start()
    {
        _sender = new MessageSender();
        _receiver = new MessageReceiver();

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.dataSource = _pattern;

        root.Q<Button>("receive-button").clicked +=
          () => AsyncUtil.Forget(RequestReceivePattern());

        root.Q<Button>("send-button").clicked +=
          () => _sender.SendPatternData(_pattern.RawData);
    }

    async Awaitable RequestReceivePattern()
    {
        try
        {
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
