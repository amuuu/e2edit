using UnityEngine.UIElements;

public static class UIHelper
{
    public static readonly string NumButtonClass = "part-select-button";
    public static readonly string NumButtonLitClass = "part-select-button-selected";
    public static readonly string StepButtonClass = "step-select-button";
    public static readonly string StepButtonLitClass = "step-select-button-selected";
    public static readonly string StepButtonDimClass = "step-select-button-on";

    public static void InvokeButton(Button button)
    {
        using (var e = new NavigationSubmitEvent() { target = button } )
            button.SendEvent(e);
    }

    public static VisualElement CreateRowContainer(VisualElement parent)
    {
        var row = new VisualElement();
        row.AddToClassList("control-row");
        parent.Add(row);
        return row;
    }

    public static VisualElement CreateStepSpacer(VisualElement parent)
    {
        var spacer = new VisualElement();
        spacer.AddToClassList("step-spacer");
        parent.Add(spacer);
        return spacer;
    }
}
