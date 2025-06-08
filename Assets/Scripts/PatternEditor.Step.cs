using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    // Style classes
    static readonly string StepButtonLitClass = "step-select-button-selected";
    static readonly string StepButtonDimClass = "step-select-button-on";

    // Step select buttons
    Button[] _stepButtons = new Button[64];

    // Step select button factory
    Button CreateStepButton(int index)
    {
        var button = new Button();
        button.AddToClassList("step-select-button");
        button.RegisterCallback<ClickEvent>(e => SelectStep(index));
        return button;
    }

    // Step spacer factory
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

    // Step page refresh (called on tab change, etc.)
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
    }
}
