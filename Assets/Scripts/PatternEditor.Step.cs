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
        RefreshStepPage();
    }

    // Delete button
    void OnStepDeleteButton()
    {
        for (var i = _pattern.StepSelect - 1; i < 63; i++)
            _pattern.GetStepRef(_pattern.PartSelect - 1, i) =
              _pattern.GetStepRef(_pattern.PartSelect - 1, i + 1);
        RefreshStepPage();
    }

    // Initialization
    void InitStepFunctions()
    {
        var copy = _uiRoot.Q<Button>("step-copy-button");
        var paste = _uiRoot.Q<Button>("step-paste-button");
        var insert = _uiRoot.Q<Button>("step-insert-button");
        var delete = _uiRoot.Q<Button>("step-delete-button");

        copy.clicked += OnStepCopyButton;
        paste.clicked += OnStepPasteButton;
        insert.clicked += OnStepInsertButton;
        delete.clicked += OnStepDeleteButton;

        _uiRoot.RegisterCallback<KeyDownEvent>(evt => {
           if (!IsStepTabActive) return;
           if (evt.keyCode == KeyCode.C) UIUtil.InvokeButton(copy);
           if (evt.keyCode == KeyCode.I) UIUtil.InvokeButton(insert);
           if (evt.keyCode == KeyCode.D) UIUtil.InvokeButton(delete);
           if (evt.keyCode == KeyCode.V) UIUtil.InvokeButton(paste);
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
