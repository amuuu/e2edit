using UnityEngine.UIElements;

public sealed class ToolbarController
{
    public ToolbarController(VisualElement root)
    {
        root.Q<Button>("receive-button").clicked += () => {
            var data = PatternDataHandler.Data;
            AsyncUtil.Forget(DeviceHandler.ReceivePatternAsync(data));
            PatternDataHandler.NotifyDataRefresh();
        };

        root.Q<Button>("send-button").clicked += () => {
            var data = PatternDataHandler.Data;
            AsyncUtil.Forget(DeviceHandler.SendPatternAsync(data));
        };
    }
}
