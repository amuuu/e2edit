using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public sealed class HotKeyHandler
{
    void MakeButtonHotKey(string key, VisualElement root, string buttonName)
    {
        var action = new InputAction(binding: $"<Keyboard>/{key}");
        action.performed += (_) => UIHelper.InvokeButton(root.Q<Button>(buttonName));
        action.Enable();
    }

    void MakePlayStopHotKey(VisualElement root)
    {
        var action = new InputAction(binding: "<Keyboard>/space");
        action.performed += (_) => {
            if (DeviceHandler.IsPlaying)
                UIHelper.InvokeButton(root.Q<Button>("stop-button"));
            else
                UIHelper.InvokeButton(root.Q<Button>("play-button"));
        };
        action.Enable();
    }

    public HotKeyHandler(VisualElement root)
    {
        MakeButtonHotKey("c", root, "step-copy-button");
        MakeButtonHotKey("x", root, "step-cut-button");
        MakeButtonHotKey("v", root, "step-paste-button");
        MakeButtonHotKey("i", root, "step-insert-button");
        MakeButtonHotKey("d", root, "step-duplicate-button");
        MakeButtonHotKey("a", root, "notes-audition-button");
        MakePlayStopHotKey(root);
    }
}
