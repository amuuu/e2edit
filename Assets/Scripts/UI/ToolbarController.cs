using UnityEngine;
using UnityEngine.UIElements;

public sealed class ToolbarController : MonoBehaviour
{
    #region UI callbacks

    void OnReceiveButtonClicked()
    {
        AsyncUtil.Forget(PatternDataHandler.ReceiveFromDeviceAsync());
        GetComponent<StepPageController>().RefreshPage();
        GetComponent<MotionPageController>().RefreshPage();
    }

    void OnSendButtonClicked()
      => AsyncUtil.Forget(PatternDataHandler.SendToDeviceAsync());

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button>("receive-button").clicked += OnReceiveButtonClicked;
        root.Q<Button>("send-button").clicked += OnSendButtonClicked;
    }

    #endregion
}
