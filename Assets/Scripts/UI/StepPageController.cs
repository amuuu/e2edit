using UnityEngine;
using UnityEngine.UIElements;

public sealed class StepPageController
{
    #region Private members

    bool IsPageActive { get; set; }

    IntegerField _repeatStepLengthField;

    #endregion

    #region Step selector

    Button[] _stepButtons = new Button[64];

    Button CreateStepButton(int index)
    {
        var button = new Button();
        button.AddToClassList(UIHelper.StepButtonClass);
        button.clicked += () => SelectStep(index);
        return button;
    }

    void SelectStep(int i)
    {
        var data = PatternDataHandler.Data;

        // Step button highlight
        var prev = _stepButtons[data.StepSelect - 1];
        var next = _stepButtons[i];

        prev.RemoveFromClassList(UIHelper.StepButtonLitClass);
        next.AddToClassList(UIHelper.StepButtonLitClass);

        // Dim light on "On" steps
        if (data.StepOnOff)
            prev.AddToClassList(UIHelper.StepButtonDimClass);
        else
            prev.RemoveFromClassList(UIHelper.StepButtonDimClass);

        // Step selection update
        data.StepSelect = i + 1;
    }

    void BuildStepSelector(VisualElement root)
    {
        var panel = root.Q("step-selector");

        // 4 rows
        for (var i = 0; i < 4; i++)
        {
            var row = UIHelper.CreateRowContainer(panel);

            // 16 steps per row
            for (var j = 0; j < 16; j++)
            {
                var index = i * 16 + j;
                _stepButtons[index] = CreateStepButton(index);
                row.Add(_stepButtons[index]);

                // Add spacer every 4 steps
                if (j % 4 == 3 && i != 15) UIHelper.CreateStepSpacer(row);
            }
        }
    }

    void RefreshStepSelector()
    {
        var data = PatternDataHandler.Data;

        // Dim light on "On" steps
        for (var i = 0; i < 64; i++)
        {
            var step = data.GetStepRef(data.PartSelect - 1, i);
            if (step.onOff > 0)
                _stepButtons[i].AddToClassList(UIHelper.StepButtonDimClass);
            else
                _stepButtons[i].RemoveFromClassList(UIHelper.StepButtonDimClass);
        }
    }

    #endregion

    #region Function buttons

    void DoCopyFunction()
    {
        ref var step = ref PatternDataHandler.Data.CurrentStep;

        GUIUtility.systemCopyBuffer = JsonUtility.ToJson(step);
    }

    void DoCutFunction()
    {
        var data = PatternDataHandler.Data;
        var part = data.PartSelect - 1;

        DoCopyFunction();

        for (var i = data.StepSelect - 1; i < 63; i++)
            data.GetStepRef(part, i) = data.GetStepRef(part, i + 1);

        RefreshStepSelector();
    }

    void DoPasteFunction()
    {
        ref var step = ref PatternDataHandler.Data.CurrentStep;
        var source = GUIUtility.systemCopyBuffer;

        try {
            step = JsonUtility.FromJson<MessageSpecs.Step>(source);
        }
        catch {
            // Simply ignore incompatible data.
        }
    }

    void DoInsertFunction()
    {
        var data = PatternDataHandler.Data;
        var part = data.PartSelect - 1;

        for (var i = 63; i > data.StepSelect - 1; i--)
            data.GetStepRef(part, i) = data.GetStepRef(part, i - 1);

        DoPasteFunction();

        RefreshStepSelector();
    }

    void DoDupPrevFunction()
    {
        var data = PatternDataHandler.Data;
        var (part, step) = (data.PartSelect - 1, data.StepSelect - 1);

        if (step > 0)
            data.GetStepRef(part, step) = data.GetStepRef(part, step - 1);
    }

    void TransposeSingle(int delta)
      => NoteUtil.Transpose(ref PatternDataHandler.Data.CurrentStep, delta);

    void TransposeAll(int delta)
    {
        var data = PatternDataHandler.Data;
        var part = data.PartSelect - 1;

        for (var i = 0; i < 64; i++)
            NoteUtil.Transpose(ref data.GetStepRef(part, i), delta);
    }

    void DoRepeatStepsFunction()
    {
        var data = PatternDataHandler.Data;
        var part = data.PartSelect - 1;

        var length = _repeatStepLengthField.value;

        for (var i = length; i < 64; i++)
            data.GetStepRef(part, i) = data.GetStepRef(part, i % length);

        RefreshStepSelector();
    }

    void DoNotesAudition()
    {
        var data = PatternDataHandler.Data;
        AsyncUtil.Forget(DeviceHandler.PlayCurrentStepAsync(data, 0.1f));
    }

    void SetUpStepFunctions(VisualElement root)
    {
        var copy        = root.Q<Button>("step-copy-button");
        var cut         = root.Q<Button>("step-cut-button");
        var paste       = root.Q<Button>("step-paste-button");
        var insert      = root.Q<Button>("step-insert-button");
        var duplicate   = root.Q<Button>("step-duplicate-button");
        var noteDown    = root.Q<Button>("step-note-down-button");
        var noteUp      = root.Q<Button>("step-note-up-button");
        var octDown     = root.Q<Button>("step-oct-down-button");
        var octUp       = root.Q<Button>("step-oct-up-button");
        var allNoteDown = root.Q<Button>("pattern-note-down-button");
        var allNoteUp   = root.Q<Button>("pattern-note-up-button");
        var allOctDown  = root.Q<Button>("pattern-oct-down-button");
        var allOctUp    = root.Q<Button>("pattern-oct-up-button");
        var repeat      = root.Q<Button>("repeat-steps-button");
        var audition    = root.Q<Button>("notes-audition-button");

        // Callback registration
        copy        .clicked += DoCopyFunction;
        cut         .clicked += DoCutFunction;
        paste       .clicked += DoPasteFunction;
        insert      .clicked += DoInsertFunction;
        duplicate   .clicked += DoDupPrevFunction;
        noteDown    .clicked += () => TransposeSingle(-1);
        noteUp      .clicked += () => TransposeSingle(1);
        octDown     .clicked += () => TransposeSingle(-12);
        octUp       .clicked += () => TransposeSingle(12);
        allNoteDown .clicked += () => TransposeAll(-1);
        allNoteUp   .clicked += () => TransposeAll(1);
        allOctDown  .clicked += () => TransposeAll(-12);
        allOctUp    .clicked += () => TransposeAll(12);
        repeat      .clicked += DoRepeatStepsFunction;
        audition    .clicked += DoNotesAudition;

        // Hotkey via key down events
        root.RegisterCallback<KeyDownEvent>(e =>
        {
           if (!IsPageActive) return;
           if (e.keyCode == KeyCode.C) UIHelper.InvokeButton(copy);
           if (e.keyCode == KeyCode.X) UIHelper.InvokeButton(cut);
           if (e.keyCode == KeyCode.V) UIHelper.InvokeButton(paste);
           if (e.keyCode == KeyCode.I) UIHelper.InvokeButton(insert);
           if (e.keyCode == KeyCode.D) UIHelper.InvokeButton(duplicate);
           if (e.keyCode == KeyCode.A) UIHelper.InvokeButton(audition);
        });
    }

    #endregion

    #region Constructor

    public StepPageController(VisualElement root)
    {
        // Control search
        var tab = root.Q<Tab>("step-tab");
        _repeatStepLengthField = root.Q<IntegerField>("step-length-field");

        // Tab change callback
        root.Q<TabView>().activeTabChanged += (prevTab, newTab) => {
            IsPageActive = (newTab == tab);
            if (IsPageActive) RefreshStepSelector();
        };

        // Pattern data refresh callback
        PatternDataHandler.OnDataRefresh += RefreshStepSelector;

        // Setting up
        BuildStepSelector(root);
        SetUpStepFunctions(root);

        // Initial state
        SelectStep(0);
    }

    #endregion
}
