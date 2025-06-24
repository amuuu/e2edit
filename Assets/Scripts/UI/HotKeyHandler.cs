using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public sealed class HotKeyHandler
{
    void MakeButtonHotKey(string key, Tab tab, string buttonName)
    {
        var button = tab.Q<Button>(buttonName);
        var action = new InputAction(binding: $"<Keyboard>/{key}");
        action.performed += (_) => {
            if (tab.panel.focusController.focusedElement != null) return;
            if ((tab.parent as TabView).activeTab != tab) return;
            UIHelper.InvokeButton(button);
        };
        action.Enable();
    }

    void MakePlayStopHotKey(VisualElement root)
    {
        var action = new InputAction(binding: "<Keyboard>/space");
        action.performed += (_) => {
            if (root.panel.focusController.focusedElement != null) return;
            if (DeviceHandler.IsPlaying)
                UIHelper.InvokeButton(root.Q<Button>("stop-button"));
            else
                UIHelper.InvokeButton(root.Q<Button>("play-button"));
        };
        action.Enable();
    }

    public HotKeyHandler(VisualElement root)
    {
        var tab = root.Q<Tab>("step-tab");
        MakeButtonHotKey("c", tab, "step-copy-button");
        MakeButtonHotKey("x", tab, "step-cut-button");
        MakeButtonHotKey("v", tab, "step-paste-button");
        MakeButtonHotKey("i", tab, "step-insert-button");
        MakeButtonHotKey("d", tab, "step-duplicate-button");
        MakeButtonHotKey("a", tab, "notes-audition-button");
        MakePlayStopHotKey(root);
    }
}
