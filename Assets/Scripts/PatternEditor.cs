using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class PatternEditor : MonoBehaviour
{
    #region Private members

    MessageSender _sender;
    MessageReceiver _receiver;
    PatternDataView _pattern = new();

    #endregion

    #region UI helpers

    VisualElement UIRoot
      => GetComponent<UIDocument>().rootVisualElement;

    Button ReceiveButton
      => UIRoot.Q<Button>("receive-button");

    Button SendButton
      => UIRoot.Q<Button>("send-button");

    Button GetPartButton(int i)
      => UIRoot.Q<Button>("part-select-button-" + i);

    Button GetStepButton(int i)
      => UIRoot.Q<Button>
           ($"step-select-button-{((i - 1) / 16) + 1}-{((i - 1) % 16) + 1}");

    #endregion

    #region Callbacks

    void SelectPattern(int i)
    {
        var prev = GetPartButton(_pattern.PartSelect);
        var next = GetPartButton(i);
        prev.RemoveFromClassList("part-select-button-selected");
        next.AddToClassList("part-select-button-selected");
        _pattern.PartSelect = i;
    }

    void SelectStep(int i)
    {
        var prev = GetStepButton(_pattern.StepSelect);
        var next = GetStepButton(i);
        prev.RemoveFromClassList("step-select-button-selected");
        next.AddToClassList("step-select-button-selected");
        _pattern.StepSelect = i;
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

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _sender = new MessageSender();
        _receiver = new MessageReceiver();

        UIRoot.dataSource = _pattern;

        ReceiveButton.clicked += () => AsyncUtil.Forget(RequestReceivePattern());
        SendButton.clicked += () => _sender.SendPatternData(_pattern.RawData);

        foreach (var i in Enumerable.Range(1, 16))
            GetPartButton(i).clicked += () => SelectPattern(i);

        foreach (var i in Enumerable.Range(1, 64))
            GetStepButton(i).clicked += () => SelectStep(i);

        SelectPattern(1);
        SelectStep(1);
    }

    void OnDestroy()
    {
        _sender?.Dispose();
        _receiver?.Dispose();
        (_sender, _receiver) = (null, null);
    }

    #endregion
}
