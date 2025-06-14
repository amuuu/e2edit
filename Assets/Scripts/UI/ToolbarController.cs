using UnityEngine;
using UnityEngine.UIElements;

public sealed class ToolbarController
{
    Button _playButton, _stopButton;

    public ToolbarController(VisualElement root)
    {
        root.Q<Button>("receive-button").clicked += async () => {
            await DeviceHandler.ReceivePatternAsync(PatternDataHandler.Data);
            PatternDataHandler.NotifyDataRefresh();
        };

        _playButton = root.Q<Button>("play-button");
        _playButton.clicked += async () => {
            await DeviceHandler.SendPatternAsync(PatternDataHandler.Data);
            DeviceHandler.ClockTempo = PatternDataHandler.Data.Tempo;
            DeviceHandler.StartPlaying();
        };

        _stopButton = root.Q<Button>("stop-button");
        _stopButton.clicked += () => {
            DeviceHandler.StopPlaying();
        };
    }

    public void TogglePlayStop()
    {
        if (DeviceHandler.IsPlaying)
            UIHelper.InvokeButton(_stopButton);
        else
            UIHelper.InvokeButton(_playButton);
    }
}
