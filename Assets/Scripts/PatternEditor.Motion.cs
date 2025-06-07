using UnityEngine;
using UnityEngine.UIElements;
using Klak.UIToolkit;

public sealed partial class PatternEditor : MonoBehaviour
{
    // Motion step control array (int field + bar)
    (ClampedIntegerField field, VisualElement bar)[] _motionSteps =
      new (ClampedIntegerField, VisualElement)[64];

    // Motion step control factory
    (ClampedIntegerField field, VisualElement bar) MakeMotionStep(int index)
    {
        // Int field
        var field = new ClampedIntegerField()
        {
            label = (index + 1).ToString(),
            lowValue = 0,
            highValue = 127
        };

        field.AddToClassList("motion-value-field");

        field.RegisterValueChangedCallback
          (e => { _pattern.SetMotionValue(index, e.newValue);
                  UpdateMotionBar(index); });

        // Bar
        var bar = new VisualElement();
        bar.AddToClassList("motion-value-bar");

        // Insert the bar under the int field
        field.Add(bar);
        bar.SendToBack();

        return (field, bar);
    }

    // Motion step spacer factory
    VisualElement MakeStepSpacer()
    {
        var spacer = new VisualElement();
        spacer.AddToClassList("step-spacer");
        return spacer;
    }

    // Motion bar height update based on motion value
    void UpdateMotionBar(int index)
        => _motionSteps[index].bar.style.top = new StyleLength
             (Length.Percent((1 - _pattern.GetMotionValue(index) / 127f) * 100));

    // Motion page refresh (called on tab change, etc.)
    void RefreshMotionPage()
    {
        for (var i = 0; i < 64; i++)
        {
            _motionSteps[i].field.SetValueWithoutNotify(_pattern.GetMotionValue(i));
            UpdateMotionBar(i);
        }
    }

    // Motion page initialization
    void InitMotionPage()
    {
        var panel = _uiRoot.Q<VisualElement>("motion-step-editor");

        // 4 bars
        for (var bar = 0; bar < 4; bar++)
        {
            // Row container
            var row = new VisualElement();
            row.AddToClassList("step-selector-row");
            panel.Add(row);

            // 16 steps
            for (int i = 0; i < 16; i++)
            {
                var index = bar * 16 + i;
                _motionSteps[index] = MakeMotionStep(index);
                row.Add(_motionSteps[index].field);

                // Spaces per four steps
                if (i % 4 == 3 && i != 15) row.Add(MakeStepSpacer());
            }
        }

        // Refresh with slot changes
        _uiRoot.Q<IntegerField>("slot-selector").
          RegisterValueChangedCallback(e => RefreshMotionPage());
    }
}
