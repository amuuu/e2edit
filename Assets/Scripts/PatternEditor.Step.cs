using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    #region Step select buttons

    // Button references
    Button[] _stepButtons = new Button[64];

    // Style classes
    static readonly string StepButtonClass = "step-select-button";
    static readonly string StepButtonLitClass = "step-select-button-selected";
    static readonly string StepButtonDimClass = "step-select-button-on";

    // Create a step select button with a callback.
    Button CreateStepButton(int index)
    {
        var button = new Button();
        button.AddToClassList(StepButtonClass);
        button.clicked += () => SelectStep(index);
        return button;
    }

    // Create a spacer element between buttons.
    VisualElement CreateStepSpacer()
    {
        var spacer = new VisualElement();
        spacer.AddToClassList("step-spacer");
        return spacer;
    }

    // Step select button callback
    void SelectStep(int i)
    {
        var prev = _stepButtons[_pattern.StepSelect - 1];
        var next = _stepButtons[i];

        prev.RemoveFromClassList(StepButtonLitClass);
        next.AddToClassList(StepButtonLitClass);

        if (_pattern.StepOnOff)
            prev.AddToClassList(StepButtonDimClass);
        else
            prev.RemoveFromClassList(StepButtonDimClass);

        _pattern.StepSelect = i + 1;
    }

    #endregion

    #region Function buttons

    // Copy button
    void OnStepCopyButton()
      => GUIUtility.systemCopyBuffer = JsonUtility.ToJson(_pattern.CurrentStep);

    // Cut button
    void OnStepCutButton()
    {
        OnStepCopyButton();
        for (var i = _pattern.StepSelect - 1; i < 63; i++)
            _pattern.GetStepRef(_pattern.PartSelect - 1, i) =
              _pattern.GetStepRef(_pattern.PartSelect - 1, i + 1);
        RefreshStepPage();
    }

    // Paste button
    void OnStepPasteButton()
    {
        try
        {
            _pattern.CurrentStep =
              JsonUtility.FromJson<MessageSpecs.Step>(GUIUtility.systemCopyBuffer);
        }
        catch { /* Simply ignores incompatible data */ }
    }

    // Insert button
    void OnStepInsertButton()
    {
        for (var i = 63; i > _pattern.StepSelect - 1; i--)
            _pattern.GetStepRef(_pattern.PartSelect - 1, i) =
              _pattern.GetStepRef(_pattern.PartSelect - 1, i - 1);
        OnStepPasteButton();
        RefreshStepPage();
    }

    // Duplicate button
    void OnStepDuplicateButton()
    {
        var (pat, step) = (_pattern.PartSelect - 1, _pattern.StepSelect - 1);
        if (step == 0) return;
        _pattern.GetStepRef(pat, step) = _pattern.GetStepRef(pat, step - 1);
    }

    // Step note transpose buttons
    void OnStepNoteTransposeButton(int delta)
      => NoteUtil.Transpose(ref _pattern.CurrentStep, delta);

    // Pattern note transpose buttons
    void OnPatternNoteTransposeButton(int delta)
    {
        var pat = _pattern.PartSelect - 1;
        for (var i = 0; i < 64; i++)
            NoteUtil.Transpose(ref _pattern.GetStepRef(pat, i), delta);
    }

    // Repeat steps button
    void OnRepeatStepsButton()
    {
        var pat = _pattern.PartSelect - 1;
        var length = _uiRoot.Q<IntegerField>("repeat-steps-length").value;
        for (var i = length; i < 64; i++)
            _pattern.GetStepRef(pat, i) = _pattern.GetStepRef(pat, i % length);
        RefreshStepPage();
    }

    // Initialization
    void InitStepFunctions()
    {
        var copy = _uiRoot.Q<Button>("step-copy-button");
        var cut = _uiRoot.Q<Button>("step-cut-button");
        var paste = _uiRoot.Q<Button>("step-paste-button");
        var insert = _uiRoot.Q<Button>("step-insert-button");
        var duplicate = _uiRoot.Q<Button>("step-duplicate-button");
        var noteDown = _uiRoot.Q<Button>("step-note-down-button");
        var noteUp = _uiRoot.Q<Button>("step-note-up-button");
        var octDown = _uiRoot.Q<Button>("step-oct-down-button");
        var octUp = _uiRoot.Q<Button>("step-oct-up-button");
        var allNoteDown = _uiRoot.Q<Button>("pattern-note-down-button");
        var allNoteUp = _uiRoot.Q<Button>("pattern-note-up-button");
        var allOctDown = _uiRoot.Q<Button>("pattern-oct-down-button");
        var allOctUp = _uiRoot.Q<Button>("pattern-oct-up-button");
        var repeat = _uiRoot.Q<Button>("repeat-steps-button");

        copy.clicked += OnStepCopyButton;
        cut.clicked += OnStepCutButton;
        paste.clicked += OnStepPasteButton;
        insert.clicked += OnStepInsertButton;
        duplicate.clicked += OnStepDuplicateButton;
        noteDown.clicked += () => OnStepNoteTransposeButton(-1);
        noteUp.clicked += () => OnStepNoteTransposeButton(1);
        octDown.clicked += () => OnStepNoteTransposeButton(-12);
        octUp.clicked += () => OnStepNoteTransposeButton(12);
        allNoteDown.clicked += () => OnPatternNoteTransposeButton(-1);
        allNoteUp.clicked += () => OnPatternNoteTransposeButton(1);
        allOctDown.clicked += () => OnPatternNoteTransposeButton(-12);
        allOctUp.clicked += () => OnPatternNoteTransposeButton(12);
        repeat.clicked += OnRepeatStepsButton;

        _uiRoot.RegisterCallback<KeyDownEvent>(evt => {
           if (!IsStepTabActive) return;
           if (evt.keyCode == KeyCode.C) UIUtil.InvokeButton(copy);
           if (evt.keyCode == KeyCode.X) UIUtil.InvokeButton(cut);
           if (evt.keyCode == KeyCode.V) UIUtil.InvokeButton(paste);
           if (evt.keyCode == KeyCode.I) UIUtil.InvokeButton(insert);
           if (evt.keyCode == KeyCode.D) UIUtil.InvokeButton(duplicate);
        });

        _uiRoot.RegisterCallback<NavigationMoveEvent>(e => {
           if (!IsStepTabActive) return;
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

    void InitStepPage()
    {
        var panel = _uiRoot.Q<VisualElement>("step-selector");

        // 4 bars
        for (var bar = 0; bar < 4; bar++)
        {
            var row = CreateRowContainer(panel);

            // 16 steps
            for (int i = 0; i < 16; i++)
            {
                var index = bar * 16 + i;
                row.Add(_stepButtons[index] = CreateStepButton(index));

                // Spaces per four steps
                if (i % 4 == 3 && i != 15) row.Add(CreateStepSpacer());
            }
        }

        SelectStep(0);

        InitStepFunctions();
    }

    void RefreshStepPage()
    {
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
