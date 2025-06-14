using UnityEngine.UIElements;

public sealed class ToolbarController
{
    public ToolbarController(VisualElement root)
    {
        root.Q<Button>("receive-button").clicked += () => {
            var data = PatternDataHandler.Data;
            AsyncUtil.Forget(DeviceHandler.ReceivePatternAsync(data));
            PatternDataHandler.NotifyDataRefresh();
            DeviceHandler.ClockTempo = PatternDataHandler.Data.Tempo;
        };

        root.Q<Button>("send-button").clicked += () => {
            var data = PatternDataHandler.Data;
            AsyncUtil.Forget(DeviceHandler.SendPatternAsync(data));
        };

        root.Q<Button>("play-button").clicked += () => {
            DeviceHandler.StartPlaying();
        };

        root.Q<Button>("stop-button").clicked += () => {
            DeviceHandler.StopPlaying();
        };
    }
}
