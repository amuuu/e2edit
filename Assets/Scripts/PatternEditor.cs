using System;
using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    #region Pattern data handlers

    PatternDataView _pattern = new();
    MessageReceiver _receiver;
    MessageSender _sender;

    #endregion

    #region UI helper

    VisualElement UIRoot
      => GetComponent<UIDocument>().rootVisualElement;

    #endregion

    #region Button callback

    void ReceivePattern()
      => AsyncUtil.Forget(RequestReceivePattern());

    async Awaitable RequestReceivePattern()
    {
        try
        {
            var count = _receiver.PatternUpdateCount;
            _sender.SendCurrentPatternDataDumpRequest();
            while (count == _receiver.PatternUpdateCount)
                await Awaitable.NextFrameAsync();
            _receiver.PatternBuffer.CopyTo(_pattern.AsBytes);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    void SendPattern()
      => _sender.SendPatternData(_pattern.AsBytes);

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _receiver = new MessageReceiver();
        _sender = new MessageSender();

        UIRoot.dataSource = _pattern;
        UIRoot.Q<Button>("receive-button").clicked += ReceivePattern;
        UIRoot.Q<Button>("send-button").clicked += SendPattern;

        InitPatternPage();
        InitStepPage();
    }

    void OnDestroy()
    {
        _sender?.Dispose();
        _receiver?.Dispose();
        (_sender, _receiver) = (null, null);
    }

    #endregion
}
