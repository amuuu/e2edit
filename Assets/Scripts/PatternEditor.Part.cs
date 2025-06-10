using UnityEngine;
using UnityEngine.UIElements;

// Pattern Editor: Part page

public sealed partial class PatternEditor : MonoBehaviour
{
    #region Part selector

    // Part button references
    Button[] _partButtons = new Button[16];

    // Part button factory
    Button CreatePartButton(int index)
    {
        var button = new Button();
        button.AddToClassList(NumButtonClass);
        button.clicked += () => SelectPart(index);
        button.text = (index + 1).ToString();
        return button;
    }

    // Part selection callback
    void SelectPart(int index)
    {
        // Part button highlight
        var prev = _partButtons[_pattern.PartSelect - 1];
        var next = _partButtons[index];
        prev.RemoveFromClassList(NumButtonLitClass);
        next.AddToClassList(NumButtonLitClass);

        // Part selection update
        _pattern.PartSelect = index + 1;
    }

    #endregion

    #region Page methods

    void InitPartPage()
    {
        var panel = _uiRoot.Q<VisualElement>("part-selector");

        // 2 rows
        for (var i = 0; i < 2; i++)
        {
            var row = CreateRowContainer(panel);

            // 8 parts per row
            for (int j = 0; j < 8; j++)
            {
                var index = i * 8 + j;
                _partButtons[index] = CreatePartButton(index);
                row.Add(_partButtons[index]);
            }
        }

        SelectPart(0);
    }

    #endregion
}
