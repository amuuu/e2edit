using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    #region Step select buttons

    // Button references
    Button[] _stepButtons = new Button[64];

    // Style classes
    static readonly string StepButtonLitClass = "step-select-button-selected";
    static readonly string StepButtonDimClass = "step-select-button-on";

    // Button factory method
    Button CreateStepButton(int index)
    {
        var button = new Button();
        button.AddToClassList("step-select-button");
        button.RegisterCallback<ClickEvent>(e => SelectStep(index));
        return button;
    }

    // Spacer factory method
    VisualElement CreateStepSpacer()
    {
        var spacer = new VisualElement();
        spacer.AddToClassList("step-spacer");
        return spacer;
    }

    // Button callback
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
    void OnStepNoteDownButton()
    {
        _pattern.StepNote1 = System.Math.Max(0, _pattern.StepNote1 - 1);
        _pattern.StepNote2 = System.Math.Max(0, _pattern.StepNote2 - 1);
        _pattern.StepNote3 = System.Math.Max(0, _pattern.StepNote3 - 1);
        _pattern.StepNote4 = System.Math.Max(0, _pattern.StepNote4 - 1);
    }

    void OnStepNoteUpButton()
    {
        _pattern.StepNote1 = System.Math.Min(128, _pattern.StepNote1 + 1);
        _pattern.StepNote2 = System.Math.Min(128, _pattern.StepNote2 + 1);
        _pattern.StepNote3 = System.Math.Min(128, _pattern.StepNote3 + 1);
        _pattern.StepNote4 = System.Math.Min(128, _pattern.StepNote4 + 1);
    }

    void OnStepOctaveDownButton()
    {
        _pattern.StepNote1 = System.Math.Max(0, _pattern.StepNote1 - 12);
        _pattern.StepNote2 = System.Math.Max(0, _pattern.StepNote2 - 12);
        _pattern.StepNote3 = System.Math.Max(0, _pattern.StepNote3 - 12);
        _pattern.StepNote4 = System.Math.Max(0, _pattern.StepNote4 - 12);
    }

    void OnStepOctaveUpButton()
    {
        _pattern.StepNote1 = System.Math.Min(128, _pattern.StepNote1 + 12);
        _pattern.StepNote2 = System.Math.Min(128, _pattern.StepNote2 + 12);
        _pattern.StepNote3 = System.Math.Min(128, _pattern.StepNote3 + 12);
        _pattern.StepNote4 = System.Math.Min(128, _pattern.StepNote4 + 12);
    }

    // Pattern note transpose buttons
    void OnPatternNoteDownButton()
    {
        for (var i = 0; i < 64; i++)
        {
            ref var step = ref _pattern.GetStepRef(_pattern.PartSelect - 1, i);
            step.noteSlot1 = (byte)System.Math.Max(0, step.noteSlot1 - 1);
            step.noteSlot2 = (byte)System.Math.Max(0, step.noteSlot2 - 1);
            step.noteSlot3 = (byte)System.Math.Max(0, step.noteSlot3 - 1);
            step.noteSlot4 = (byte)System.Math.Max(0, step.noteSlot4 - 1);
        }
    }

    void OnPatternNoteUpButton()
    {
        for (var i = 0; i < 64; i++)
        {
            ref var step = ref _pattern.GetStepRef(_pattern.PartSelect - 1, i);
            step.noteSlot1 = (byte)System.Math.Min(128, step.noteSlot1 + 1);
            step.noteSlot2 = (byte)System.Math.Min(128, step.noteSlot2 + 1);
            step.noteSlot3 = (byte)System.Math.Min(128, step.noteSlot3 + 1);
            step.noteSlot4 = (byte)System.Math.Min(128, step.noteSlot4 + 1);
        }
    }

    void OnPatternOctaveDownButton()
    {
        for (var i = 0; i < 64; i++)
        {
            ref var step = ref _pattern.GetStepRef(_pattern.PartSelect - 1, i);
            step.noteSlot1 = (byte)System.Math.Max(0, step.noteSlot1 - 12);
            step.noteSlot2 = (byte)System.Math.Max(0, step.noteSlot2 - 12);
            step.noteSlot3 = (byte)System.Math.Max(0, step.noteSlot3 - 12);
            step.noteSlot4 = (byte)System.Math.Max(0, step.noteSlot4 - 12);
        }
    }

    void OnPatternOctaveUpButton()
    {
        for (var i = 0; i < 64; i++)
        {
            ref var step = ref _pattern.GetStepRef(_pattern.PartSelect - 1, i);
            step.noteSlot1 = (byte)System.Math.Min(128, step.noteSlot1 + 12);
            step.noteSlot2 = (byte)System.Math.Min(128, step.noteSlot2 + 12);
            step.noteSlot3 = (byte)System.Math.Min(128, step.noteSlot3 + 12);
            step.noteSlot4 = (byte)System.Math.Min(128, step.noteSlot4 + 12);
        }
    }

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
        noteDown.clicked += OnStepNoteDownButton;
        noteUp.clicked += OnStepNoteUpButton;
        octDown.clicked += OnStepOctaveDownButton;
        octUp.clicked += OnStepOctaveUpButton;
        allNoteDown.clicked += OnPatternNoteDownButton;
        allNoteUp.clicked += OnPatternNoteUpButton;
        allOctDown.clicked += OnPatternOctaveDownButton;
        allOctUp.clicked += OnPatternOctaveUpButton;
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
            // Row container
            var row = new VisualElement();
            row.AddToClassList("control-row");
            panel.Add(row);

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
