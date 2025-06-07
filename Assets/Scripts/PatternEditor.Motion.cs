using UnityEngine;
using UnityEngine.UIElements;
using Klak.UIToolkit;

public sealed partial class PatternEditor : MonoBehaviour
{
    ClampedIntegerField[] _motionFields = new ClampedIntegerField[64];

    void OnMotionValueChanged(int index, int value)
      => _pattern.SetMotionValue(index, value);

    void RefreshMotionPage()
    {
        for (var i = 0; i < 64; i++)
            _motionFields[i].SetValueWithoutNotify(_pattern.GetMotionValue(i));
    }

    void InitMotionPage()
    {
        var panel = _uiRoot.Q<VisualElement>("motion-step-editor");

        for (var bar = 0; bar < 4; bar++)
        {
            var row = new VisualElement();
            row.AddToClassList("step-selector-row");
            panel.Add(row);

            for (int i = 0; i < 16; i++)
            {
                var index = bar * 16 + i;
                var field = new ClampedIntegerField()
                {
                    label = (index + 1).ToString(),
                    lowValue = 0, highValue = 127
                };
                field.RegisterValueChangedCallback
                  (e => OnMotionValueChanged(index, e.newValue));
                row.Add(field);
                _motionFields[index] = field;
            }
        }

        _uiRoot.Q<IntegerField>("slot-selector").
          RegisterValueChangedCallback(e => RefreshMotionPage());
    }
}
