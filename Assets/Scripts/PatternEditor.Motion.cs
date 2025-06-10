using UnityEngine;
using UnityEngine.UIElements;
using Klak.UIToolkit;

public sealed partial class PatternEditor : MonoBehaviour
{
    #region Motion slot select buttons

    // Button references
    Button[] _motionSlotButtons = new Button[24];

    // Button factory method
    Button CreateMotionSlotButton(int index)
    {
        var button = new Button();
        button.AddToClassList(PartButtonClass);
        button.clicked += () => SelectMotionSlot(index);
        button.text = (index + 1).ToString();
        return button;
    }

    // Button callback
    void SelectMotionSlot(int index)
    {
        var prev = _motionSlotButtons[_pattern.MotionSelect - 1];
        var next = _motionSlotButtons[index];

        prev.RemoveFromClassList(PartButtonLitClass);
        next.AddToClassList(PartButtonLitClass);

        _pattern.MotionSelect = index + 1;

        RefreshMotionPage();
    }

    #endregion

    #region Motion step controls

    // Motion step control array (int field + bar)
    (ClampedIntegerField field, VisualElement bar)[] _motionSteps =
      new (ClampedIntegerField, VisualElement)[64];

    // Motion step control factory
    (ClampedIntegerField field, VisualElement bar) CreateMotionStep(int index)
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

    // Motion bar height update based on motion value
    void UpdateMotionBar(int index)
        => _motionSteps[index].bar.style.top = new StyleLength
             (Length.Percent((1 - _pattern.GetMotionValue(index) / 127f) * 100));

    #endregion

    #region Page methods

    void InitMotionPage()
    {
        // -- Motion slot selector initialization
        var panel = _uiRoot.Q<VisualElement>("motion-slot-selector");

        // 2 rows
        for (var i = 0; i < 2; i++)
        {
            // Row container
            var row = new VisualElement();
            row.AddToClassList(ControlRowClass);
            panel.Add(row);

            // 12 slots per row
            for (int j = 0; j < 12; j++)
            {
                var index = i * 12 + j;
                row.Add(_motionSlotButtons[index] = CreateMotionSlotButton(index));
            }
        }

        // -- Motion step control initialization
        panel = _uiRoot.Q<VisualElement>("motion-step-editor");

        // 4 bars
        for (var bar = 0; bar < 4; bar++)
        {
            // Row container
            var row = new VisualElement();
            row.AddToClassList(ControlRowClass);
            panel.Add(row);

            // 16 steps
            for (int i = 0; i < 16; i++)
            {
                var index = bar * 16 + i;
                _motionSteps[index] = CreateMotionStep(index);
                row.Add(_motionSteps[index].field);

                // Spaces per four steps
                if (i % 4 == 3 && i != 15) row.Add(CreateStepSpacer());
            }
        }

        SelectMotionSlot(0);
    }

    void RefreshMotionPage()
    {
        for (var i = 0; i < 64; i++)
        {
            _motionSteps[i].field.SetValueWithoutNotify(_pattern.GetMotionValue(i));
            UpdateMotionBar(i);
        }
    }

    #endregion
}
