using UnityEngine;
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

        root.Q<Button>("play-button").clicked += () =>
            AsyncUtil.Forget(SendAndPlayAsync());

        root.Q<Button>("stop-button").clicked += () =>
            DeviceHandler.StopPlaying();

        root.RegisterCallback<KeyDownEvent>(e => {
            if (e.keyCode == KeyCode.Space)
            {
                if (DeviceHandler.IsPlaying)
                    DeviceHandler.StopPlaying();
                else
                    AsyncUtil.Forget(SendAndPlayAsync());
            }
        });
    }

    async Awaitable SendAndPlayAsync()
    {
        await DeviceHandler.SendPatternAsync(PatternDataHandler.Data);
        DeviceHandler.ClockTempo = PatternDataHandler.Data.Tempo;
        DeviceHandler.StartPlaying();
    }
}
