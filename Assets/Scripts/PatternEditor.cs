using System;
using UnityEngine;
using UnityEngine.UIElements;

public sealed partial class PatternEditor : MonoBehaviour
{
    #region Pattern data handlers

    PatternDataView _pattern = new();
    MessageReceiver _receiver;
    MessageSender _sender;

    #endregion

    #region UI elements

    VisualElement _uiRoot;
    (TabView parent, Tab pattern, Tab part, Tab step, Tab motion) _pages;

    #endregion

    #region Common UI helpers

    static readonly string NumButtonClass = "part-select-button";
    static readonly string NumButtonLitClass = "part-select-button-selected";

    bool IsStepPageActive
      => _pages.parent.activeTab == _pages.step;

    VisualElement CreateRowContainer(VisualElement parent)
    {
        var row = new VisualElement();
        row.AddToClassList("control-row");
        parent.Add(row);
        return row;
    }

    VisualElement CreateStepSpacer(VisualElement parent)
    {
        var spacer = new VisualElement();
        spacer.AddToClassList("step-spacer");
        parent.Add(spacer);
        return spacer;
    }

    #endregion

    #region UI callbacks

    void OnReceiveButton()
      => AsyncUtil.Forget(RequestReceivePattern());

    async Awaitable RequestReceivePattern()
    {
        try
        {
            var count = _receiver.PatternUpdateCount;
            _sender.SendCurrentPatternDataDumpRequest();
            while (count == _receiver.PatternUpdateCount)
                await Awaitable.NextFrameAsync();
            _receiver.PatternBuffer.CopyTo(_pattern.AsBytes);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        RefreshStepPage();
        RefreshMotionPage();
    }

    void OnSendButton()
      => _sender.SendPatternData(_pattern.AsBytes);

    void OnTabChanged(Tab prevTab, Tab newTab)
    {
        if (newTab == _pages.part) RefreshPartPage();
        if (newTab == _pages.step) RefreshStepPage();
        if (newTab == _pages.motion) RefreshMotionPage();
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _receiver = new MessageReceiver();
        _sender = new MessageSender();

        _uiRoot = GetComponent<UIDocument>().rootVisualElement;
        _uiRoot.dataSource = _pattern;
        _uiRoot.Q<Button>("receive-button").clicked += OnReceiveButton;
        _uiRoot.Q<Button>("send-button").clicked += OnSendButton;
        _uiRoot.Q<TabView>().activeTabChanged += OnTabChanged;

        _pages.parent = _uiRoot.Q<TabView>();
        _pages.pattern = _uiRoot.Q<Tab>("pattern-tab");
        _pages.part = _uiRoot.Q<Tab>("part-tab");
        _pages.step = _uiRoot.Q<Tab>("step-tab");
        _pages.motion = _uiRoot.Q<Tab>("motion-tab");

        SetUpPartPage();
        SetUpStepPage();
        SetUpMotionPage();
    }

    void OnDestroy()
    {
        _sender?.Dispose();
        _receiver?.Dispose();
        (_sender, _receiver) = (null, null);
    }

    #endregion
}
