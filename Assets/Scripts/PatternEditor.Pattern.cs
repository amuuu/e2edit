using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    #region Part select buttons

    // Button references
    Button[] _partButtons = new Button[16];

    // Style classes
    static readonly string ControlRowClass = "control-row";
    static readonly string PartButtonClass = "part-select-button";
    static readonly string PartButtonLitClass = "part-select-button-selected";

    // Button factory method
    Button CreatePartButton(int index)
    {
        var button = new Button();
        button.AddToClassList(PartButtonClass);
        button.clicked += () => SelectPart(index);
        button.text = (index + 1).ToString();
        return button;
    }

    // Button callback
    void SelectPart(int index)
    {
        var prev = _partButtons[_pattern.PartSelect - 1];
        var next = _partButtons[index];

        prev.RemoveFromClassList(PartButtonLitClass);
        next.AddToClassList(PartButtonLitClass);

        _pattern.PartSelect = index + 1;
    }

    #endregion

    #region Page methods

    void InitPatternPage()
    {
        var panel = _uiRoot.Q<VisualElement>("part-selector");

        // 2 rows
        for (var i = 0; i < 2; i++)
        {
            // Row container
            var row = new VisualElement();
            row.AddToClassList(ControlRowClass);
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

    #endregion
}
