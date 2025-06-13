public static class PatternDataHandler
{
    public static PatternDataView Data
      { get; private set; } = new PatternDataView();

    public static event System.Action OnDataRefresh;

    public static void NotifyDataRefresh() => OnDataRefresh.Invoke();
}
