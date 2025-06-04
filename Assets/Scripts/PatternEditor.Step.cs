using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    Button GetStepButton(int i)
      => UIRoot.Q<Button>
           ($"step-select-button-{((i - 1) / 16) + 1}-{((i - 1) % 16) + 1}");

    void SelectStep(int i)
    {
        var prev = GetStepButton(_pattern.StepSelect);
        var next = GetStepButton(i);
        prev.RemoveFromClassList("step-select-button-selected");
        next.AddToClassList("step-select-button-selected");
        _pattern.StepSelect = i;
    }

    void InitStepPage()
    {
        foreach (var i in Enumerable.Range(1, 64))
            GetStepButton(i).clicked += () => SelectStep(i);
        SelectStep(1);
    }
}
