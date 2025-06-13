using UnityEngine.UIElements;
using Klak.UIToolkit;

public sealed class MotionPageController
{
    #region Private members

    bool IsPageActive { get; set; }

    #endregion

    #region Motion slot selector

    Button[] _slotButtons = new Button[24];

    Button CreateSlotButton(int index)
    {
        var button = new Button();
        button.AddToClassList(UIHelper.NumButtonClass);
        button.clicked += () => SelectSlot(index);
        button.text = (index + 1).ToString();
        return button;
    }

    void SelectSlot(int index)
    {
        var data = PatternDataHandler.Data;

        // Slot button highlight
        var prev = _slotButtons[data.MotionSelect - 1];
        var next = _slotButtons[index];

        prev.RemoveFromClassList(UIHelper.NumButtonLitClass);
        next.AddToClassList(UIHelper.NumButtonLitClass);

        // Slot selection update
        data.MotionSelect = index + 1;

        // Refresh page to sync with step controls.
        RefreshStepEditor();
    }

    void BuildSlotSelector(VisualElement root)
    {
        var panel = root.Q("motion-slot-selector");

        // 2 rows
        for (var i = 0; i < 2; i++)
        {
            var row = UIHelper.CreateRowContainer(panel);

            // 12 slots per row
            for (var j = 0; j < 12; j++)
            {
                var index = i * 12 + j;
                _slotButtons[index] = CreateSlotButton(index);
                row.Add(_slotButtons[index]);
            }
        }
    }

    #endregion

    #region Motion step controls

    // Step control array (int field + bar)
    (ClampedIntegerField field, VisualElement bar)[] _steps =
      new (ClampedIntegerField, VisualElement)[64];

    // Step control factory
    (ClampedIntegerField field, VisualElement bar) CreateStep(int index)
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
            PatternDataHandler.Data.SetMotionValue(index, e.newValue);
            UpdateStepBar(index);
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
    void UpdateStepBar(int index)
    {
        var data = PatternDataHandler.Data;
        var height = (1 - data.GetMotionValue(index) / 127.0f) * 100;
        _steps[index].bar.style.top = new StyleLength(Length.Percent(height));
    }

    // Step editor builder
    void BuildStepEditor(VisualElement root)
    {
        var panel = root.Q("motion-step-editor");

        // 4 rows
        for (var i = 0; i < 4; i++)
        {
            var row = UIHelper.CreateRowContainer(panel);

            // 16 steps per row
            for (var j = 0; j < 16; j++)
            {
                var index = i * 16 + j;
                _steps[index] = CreateStep(index);
                row.Add(_steps[index].field);

                // Add spacer every 4 steps
                if (j % 4 == 3 && i != 15) UIHelper.CreateStepSpacer(row);
            }
        }
    }

    void RefreshStepEditor()
    {
        var data = PatternDataHandler.Data;

        for (var i = 0; i < 64; i++)
        {
            _steps[i].field.SetValueWithoutNotify(data.GetMotionValue(i));
            UpdateStepBar(i);
        }
    }

    #endregion

    #region Constructor

    public MotionPageController(VisualElement root)
    {
        // Control search
        var tab = root.Q<Tab>("motion-tab");

        // Tab change callback
        root.Q<TabView>().activeTabChanged += (prevTab, newTab) => {
            IsPageActive = (newTab == tab);
            if (IsPageActive) RefreshStepEditor();
        };

        // Pattern data refresh callback
        PatternDataHandler.OnDataRefresh += RefreshStepEditor;

        // Setting up
        BuildSlotSelector(root);
        BuildStepEditor(root);

        // Initial state
        SelectSlot(0);
    }

    #endregion
}
