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
    HotKeyHandler _hotKey;

    void OnEnable()
    {
        GlobalStringTable.Initialize(_stringTable.Data);

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.dataSource = PatternDataHandler.Data;

        _toolbar = new ToolbarController(root);
        _partPage = new PartPageController(root);
        _stepPage = new StepPageController(root);
        _motionPage = new MotionPageController(root);
        _hotKey = new HotKeyHandler(root);

        DeviceHandler.SetUp();
    }

    void OnDisable()
    {
        _toolbar = null;
        _partPage = null;
        _stepPage = null;
        _motionPage = null;
        _hotKey = null;

        DeviceHandler.TearDown();
    }

    async Awaitable Start()
    {
        await DeviceHandler.ReceivePatternAsync(PatternDataHandler.Data);
        PatternDataHandler.NotifyDataRefresh();
    }
}
