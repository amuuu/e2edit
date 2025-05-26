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

        for (var i = 1; i <= 16; i++)
        {
            var button = root.Q<Button>("part-select-button-" + i);
            var temp = i;
            button.clicked += () => SelectPattern(temp);
        }

        for (var i = 1; i <= 64; i++)
        {
            var temp = i;
            GetStepButton(i).clicked += () => SelectStep(temp);
        }

        SelectPattern(1);
        SelectStep(1);
    }

    Button GetStepButton(int i)
      => GetComponent<UIDocument>().rootVisualElement.Q<Button>
           ($"step-select-button-{((i - 1) / 16) + 1}-{((i - 1) % 16) + 1}");

    void SelectPattern(int i)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var prev = root.Q<Button>("part-select-button-" + _pattern.PartSelect);
        var next = root.Q<Button>("part-select-button-" + i);
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

    void OnDestroy()
    {
        _sender?.Dispose();
        _receiver?.Dispose();
        (_sender, _receiver) = (null, null);
    }
}
