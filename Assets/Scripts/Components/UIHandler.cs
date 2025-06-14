using UnityEngine;
using UnityEngine.UIElements;

public sealed class UIHandler : MonoBehaviour
{
    [SerializeField] ScriptableStringTable _stringTable = null;

    void OnEnable()
    {
        GlobalStringTable.Initialize(_stringTable.Data);

        var root = GetComponent<UIDocument>().rootVisualElement;
        root.dataSource = PatternDataHandler.Data;

        new ToolbarController(root);
        new PartPageController(root);
        new StepPageController(root);
        new MotionPageController(root);

        DeviceHandler.SetUp();
    }

    void OnDisable()
      => DeviceHandler.TearDown();
}
