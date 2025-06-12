using UnityEngine.UIElements;

public sealed class ToolbarController
{
    public ToolbarController(VisualElement root)
    {
        root.Q<Button>("receive-button").clicked +=
          () => AsyncUtil.Forget(PatternDataHandler.ReceiveFromDeviceAsync());

        root.Q<Button>("send-button").clicked +=
          () => AsyncUtil.Forget(PatternDataHandler.SendToDeviceAsync());
    }
}
