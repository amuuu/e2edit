using UnityEngine;
using UnityEngine.UIElements;

public sealed class UIHandler : MonoBehaviour
{
#pragma warning disable CS0414
    [SerializeField] StringTable _stringTable = null;

    ToolbarController _toolbar;
    PartPageController _part;
    StepPageController _step;
    MotionPageController _motion;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.dataSource = PatternDataHandler.Data;
        _toolbar = new ToolbarController(root);
        _part = new PartPageController(root);
        _step = new StepPageController(root);
        _motion = new MotionPageController(root);
    }
}
