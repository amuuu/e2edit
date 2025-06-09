using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    // Part button references
    Button[] _partButtons = new Button[16];

    // Part button factory method
    Button CreatePartButton(int index)
    {
        var button = new Button();
        button.AddToClassList("part-select-button");
        button.clicked += () => SelectPart(index);
        button.text = (index + 1).ToString();
        return button;
    }

    // Part button callback
    void SelectPart(int index)
    {
        var prev = _partButtons[_pattern.PartSelect - 1];
        var next = _partButtons[index];

        prev.RemoveFromClassList("part-select-button-selected");
        next.AddToClassList("part-select-button-selected");

        _pattern.PartSelect = index + 1;
    }

    // Tab initialization
    void InitPatternPage()
    {
        var panel = _uiRoot.Q<VisualElement>("part-selector");

        // 2 rows
        for (var i = 0; i < 2; i++)
        {
            // Row container
            var row = new VisualElement();
            row.AddToClassList("control-row");
            panel.Add(row);

            // 8 parts per row
            for (int j = 0; j < 8; j++)
            {
                var index = i * 8 + j;
                row.Add(_partButtons[index] = CreatePartButton(index));
            }
        }

        SelectPart(0);
    }
}
