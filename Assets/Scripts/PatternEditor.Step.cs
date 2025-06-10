using UnityEngine;
using UnityEngine.UIElements;

// Pattern Editor: Step page

public sealed partial class PatternEditor : MonoBehaviour
{
    #region Step selector

    // Step button references
    Button[] _stepButtons = new Button[64];

    // Style classes
    static readonly string StepButtonClass = "step-select-button";
    static readonly string StepButtonLitClass = "step-select-button-selected";
    static readonly string StepButtonDimClass = "step-select-button-on";

    // Step button factory
    Button CreateStepButton(int index)
    {
        var button = new Button();
        button.AddToClassList(StepButtonClass);
        button.clicked += () => SelectStep(index);
        return button;
    }

    // Step select button callback
    void SelectStep(int i)
    {
        // Step button highlight
        var prev = _stepButtons[_pattern.StepSelect - 1];
        var next = _stepButtons[i];
        prev.RemoveFromClassList(StepButtonLitClass);
        next.AddToClassList(StepButtonLitClass);

        // Dim light on "On" steps
        if (_pattern.StepOnOff)
            prev.AddToClassList(StepButtonDimClass);
        else
            prev.RemoveFromClassList(StepButtonDimClass);

        // Step selection update
        _pattern.StepSelect = i + 1;
    }

    // Selector builder
    void BuildStepSelector()
    {
        var panel = _uiRoot.Q<VisualElement>("step-selector");

        // 4 rows
        for (var i = 0; i < 4; i++)
        {
            var row = CreateRowContainer(panel);

            // 16 steps per row
            for (var j = 0; j < 16; j++)
            {
                var index = i * 16 + j;
                row.Add(_stepButtons[index] = CreateStepButton(index));

                // Add spacer every 4 steps
                if (j % 4 == 3 && i != 15) CreateStepSpacer(row);
            }
        }
    }

    #endregion

    #region Function buttons

    // "Copy"
    void OnStepCopyButton()
      => GUIUtility.systemCopyBuffer = JsonUtility.ToJson(_pattern.CurrentStep);

    // "Cut"
    void OnStepCutButton()
    {
        OnStepCopyButton();
        var part = _pattern.PartSelect - 1;
        for (var i = _pattern.StepSelect - 1; i < 63; i++)
            _pattern.GetStepRef(part, i) = _pattern.GetStepRef(part, i + 1);
        RefreshStepPage();
    }

    // "Paste"
    void OnStepPasteButton()
    {
        try
        {
            _pattern.CurrentStep =
              JsonUtility.FromJson<MessageSpecs.Step>(GUIUtility.systemCopyBuffer);
        }
        catch { /* Simply ignore incompatible data. */ }
    }

    // "Insert"
    void OnStepInsertButton()
    {
        var part = _pattern.PartSelect - 1;
        for (var i = 63; i > _pattern.StepSelect - 1; i--)
            _pattern.GetStepRef(part, i) = _pattern.GetStepRef(part, i - 1);
        OnStepPasteButton();
        RefreshStepPage();
    }

    // "Dup Prev"
    void OnStepDuplicateButton()
    {
        if (_pattern.StepSelect == 1) return;
        var (part, step) = (_pattern.PartSelect - 1, _pattern.StepSelect - 1);
        _pattern.GetStepRef(part, step) = _pattern.GetStepRef(part, step - 1);
    }

    // Transpose buttons
    void OnStepNoteTransposeButton(int delta)
      => NoteUtil.Transpose(ref _pattern.CurrentStep, delta);

    // Transpose-all buttons
    void OnPatternNoteTransposeButton(int delta)
    {
        var part = _pattern.PartSelect - 1;
        for (var i = 0; i < 64; i++)
            NoteUtil.Transpose(ref _pattern.GetStepRef(part, i), delta);
    }

    // Repeat steps button
    void OnRepeatStepsButton()
    {
        var part = _pattern.PartSelect - 1;
        var length = _uiRoot.Q<IntegerField>("repeat-steps-length").value;
        for (var i = length; i < 64; i++)
            _pattern.GetStepRef(part, i) = _pattern.GetStepRef(part, i % length);
        RefreshStepPage();
    }

    // Setting up
    void SetUpStepFunctions()
    {
        var copy        = _uiRoot.Q<Button>("step-copy-button");
        var cut         = _uiRoot.Q<Button>("step-cut-button");
        var paste       = _uiRoot.Q<Button>("step-paste-button");
        var insert      = _uiRoot.Q<Button>("step-insert-button");
        var duplicate   = _uiRoot.Q<Button>("step-duplicate-button");
        var noteDown    = _uiRoot.Q<Button>("step-note-down-button");
        var noteUp      = _uiRoot.Q<Button>("step-note-up-button");
        var octDown     = _uiRoot.Q<Button>("step-oct-down-button");
        var octUp       = _uiRoot.Q<Button>("step-oct-up-button");
        var allNoteDown = _uiRoot.Q<Button>("pattern-note-down-button");
        var allNoteUp   = _uiRoot.Q<Button>("pattern-note-up-button");
        var allOctDown  = _uiRoot.Q<Button>("pattern-oct-down-button");
        var allOctUp    = _uiRoot.Q<Button>("pattern-oct-up-button");
        var repeat      = _uiRoot.Q<Button>("repeat-steps-button");

        // Callback registration
        copy        .clicked += OnStepCopyButton;
        cut         .clicked += OnStepCutButton;
        paste       .clicked += OnStepPasteButton;
        insert      .clicked += OnStepInsertButton;
        duplicate   .clicked += OnStepDuplicateButton;
        noteDown    .clicked += () => OnStepNoteTransposeButton(-1);
        noteUp      .clicked += () => OnStepNoteTransposeButton(1);
        octDown     .clicked += () => OnStepNoteTransposeButton(-12);
        octUp       .clicked += () => OnStepNoteTransposeButton(12);
        allNoteDown .clicked += () => OnPatternNoteTransposeButton(-1);
        allNoteUp   .clicked += () => OnPatternNoteTransposeButton(1);
        allOctDown  .clicked += () => OnPatternNoteTransposeButton(-12);
        allOctUp    .clicked += () => OnPatternNoteTransposeButton(12);
        repeat      .clicked += OnRepeatStepsButton;

        // Hotkey via key down events
        _uiRoot.RegisterCallback<KeyDownEvent>(e =>
        {
           if (!IsStepPageActive) return;
           if (e.keyCode == KeyCode.C) UIUtil.InvokeButton(copy);
           if (e.keyCode == KeyCode.X) UIUtil.InvokeButton(cut);
           if (e.keyCode == KeyCode.V) UIUtil.InvokeButton(paste);
           if (e.keyCode == KeyCode.I) UIUtil.InvokeButton(insert);
           if (e.keyCode == KeyCode.D) UIUtil.InvokeButton(duplicate);
        });

        // Disable navigation by keyboard
        _uiRoot.RegisterCallback<NavigationMoveEvent>(e =>
        {
           if (!IsStepPageActive) return;
           if (e.direction is not
               (NavigationMoveEvent.Direction.Up or
                NavigationMoveEvent.Direction.Down or
                NavigationMoveEvent.Direction.Left or
                NavigationMoveEvent.Direction.Right)) return;
           e.StopPropagation();
           _uiRoot.focusController.IgnoreEvent(e);
         });
    }

    #endregion

    #region Page methods

    void SetUpStepPage()
    {
        BuildStepSelector();
        SetUpStepFunctions();
        SelectStep(0);
    }

    void RefreshStepPage()
    {
        // Dim light on "On" steps
        for (var i = 0; i < 64; i++)
        {
            var step = _pattern.GetStepRef(_pattern.PartSelect - 1, i);
            if (step.onOff > 0)
                _stepButtons[i].AddToClassList(StepButtonDimClass);
            else
                _stepButtons[i].RemoveFromClassList(StepButtonDimClass);
        }
    }

    #endregion
}
