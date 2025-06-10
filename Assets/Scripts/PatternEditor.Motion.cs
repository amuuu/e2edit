using UnityEngine;
using UnityEngine.UIElements;
using Klak.UIToolkit;

// Pattern Editor: Motion page

public sealed partial class PatternEditor : MonoBehaviour
{
    #region Motion slot selector

    // Slot button references
    Button[] _motionSlotButtons = new Button[24];

    // Slot button factory
    Button CreateMotionSlotButton(int index)
    {
        var button = new Button();
        button.AddToClassList(NumButtonClass);
        button.clicked += () => SelectMotionSlot(index);
        button.text = (index + 1).ToString();
        return button;
    }

    // Slot selection callback
    void SelectMotionSlot(int index)
    {
        // Slot button highlight
        var prev = _motionSlotButtons[_pattern.MotionSelect - 1];
        var next = _motionSlotButtons[index];
        prev.RemoveFromClassList(NumButtonLitClass);
        next.AddToClassList(NumButtonLitClass);

        // Slot selection update
        _pattern.MotionSelect = index + 1;

        // Refresh the entire page to sync with the motion step controls.
        RefreshMotionPage();
    }

    // Selector builder
    void BuildMotionSlotSelector()
    {
        var panel = _uiRoot.Q<VisualElement>("motion-slot-selector");

        // 2 rows
        for (var i = 0; i < 2; i++)
        {
            var row = CreateRowContainer(panel);

            // 12 slots per row
            for (var j = 0; j < 12; j++)
            {
                var index = i * 12 + j;
                _motionSlotButtons[index] = CreateMotionSlotButton(index);
                row.Add(_motionSlotButtons[index]);
            }
        }
    }

    #endregion

    #region Motion step controls

    // Step control array (int field + bar)
    (ClampedIntegerField field, VisualElement bar)[]
      _motionSteps = new (ClampedIntegerField, VisualElement)[64];

    // Step control factory
    (ClampedIntegerField field, VisualElement bar) CreateMotionStep(int index)
    {
        // Integer value field
        var field = new ClampedIntegerField()
        {
            label = (index + 1).ToString(),
            lowValue = 0,
            highValue = 127
        };
        field.AddToClassList("motion-value-field");

        // Value change callback
        field.RegisterValueChangedCallback(e =>
        {
            _pattern.SetMotionValue(index, e.newValue);
            UpdateMotionStepBar(index);
        });

        // Bar
        var bar = new VisualElement();
        bar.AddToClassList("motion-value-bar");

        // Insert bar under the value field
        field.Add(bar);
        bar.SendToBack();

        return (field, bar);
    }

    // Bar height update
    void UpdateMotionStepBar(int index)
        => _motionSteps[index].bar.style.top = new StyleLength
             (Length.Percent((1 - _pattern.GetMotionValue(index) / 127f) * 100));

    // Step editor builder
    void BuildMotionStepEditor()
    {
        var panel = _uiRoot.Q<VisualElement>("motion-step-editor");

        // 4 rows
        for (var i = 0; i < 4; i++)
        {
            var row = CreateRowContainer(panel);

            // 16 steps per row
            for (var j = 0; j < 16; j++)
            {
                var index = i * 16 + j;
                _motionSteps[index] = CreateMotionStep(index);
                row.Add(_motionSteps[index].field);

                // Add spacer every 4 steps
                if (j % 4 == 3 && i != 15) CreateStepSpacer(row);
            }
        }
    }

    #endregion

    #region Page methods

    void SetUpMotionPage()
    {
        BuildMotionSlotSelector();
        BuildMotionStepEditor();
        SelectMotionSlot(0);
    }

    void RefreshMotionPage()
    {
        for (var i = 0; i < 64; i++)
        {
            _motionSteps[i].field.SetValueWithoutNotify(_pattern.GetMotionValue(i));
            UpdateMotionStepBar(i);
        }
    }

    #endregion
}
