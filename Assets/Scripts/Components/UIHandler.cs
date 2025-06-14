using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public sealed class UIHandler : MonoBehaviour
{
    [SerializeField] ScriptableStringTable _stringTable = null;

    ToolbarController _toolbar;
    PartPageController _partPage;
    StepPageController _stepPage;
    MotionPageController _motionPage;

    void OnEnable()
    {
        GlobalStringTable.Initialize(_stringTable.Data);

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.dataSource = PatternDataHandler.Data;

        _toolbar = new ToolbarController(root);
        _partPage = new PartPageController(root);
        _stepPage = new StepPageController(root);
        _motionPage = new MotionPageController(root);

        DeviceHandler.SetUp();
    }

    void OnDisable()
    {
        _toolbar = null;
        _partPage = null;
        _stepPage = null;
        _motionPage = null;

        DeviceHandler.TearDown();
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            _toolbar.TogglePlayStop();
    }
}
