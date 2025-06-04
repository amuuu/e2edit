using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    Button[] _stepButtons = new Button[64];

    void SelectStep(int i)
    {
        var prev = _stepButtons[_pattern.StepSelect - 1];
        var next = _stepButtons[i];
        prev.RemoveFromClassList("step-select-button-selected");
        next.AddToClassList("step-select-button-selected");
        if (_pattern.StepOnOff)
            prev.AddToClassList("step-select-button-on");
        else
            prev.RemoveFromClassList("step-select-button-on");
        _pattern.StepSelect = i + 1;
    }

    void RefreshStepPage()
    {
        for (var i = 0; i < 64; i++)
        {
            var step = _pattern.GetStepRef(_pattern.PartSelect - 1, i);
            if (step.onOff > 0)
                _stepButtons[i].AddToClassList("step-select-button-on");
            else
                _stepButtons[i].RemoveFromClassList("step-select-button-on");
        }
    }

    void InitStepPage()
    {
        foreach (var i in Enumerable.Range(0, 64))
        {
            var name = $"step-select-button-{(i / 16) + 1}-{(i % 16) + 1}";
            _stepButtons[i] = _uiRoot.Q<Button>(name);
            _stepButtons[i].clicked += () => SelectStep(i);
        }

        SelectStep(0);
    }
}
