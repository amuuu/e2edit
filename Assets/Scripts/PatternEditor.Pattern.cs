using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    Button GetPartButton(int i)
      => _uiRoot.Q<Button>("part-select-button-" + i);

    void SelectPattern(int i)
    {
        var prev = GetPartButton(_pattern.PartSelect);
        var next = GetPartButton(i);
        prev.RemoveFromClassList("part-select-button-selected");
        next.AddToClassList("part-select-button-selected");
        _pattern.PartSelect = i;
    }

    void InitPatternPage()
    {
        foreach (var i in Enumerable.Range(1, 16))
            GetPartButton(i).clicked += () => SelectPattern(i);
        SelectPattern(1);
    }
}
