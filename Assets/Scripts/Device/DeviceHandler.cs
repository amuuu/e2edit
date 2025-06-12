public static class DeviceHandler
{
    #region Public accessors

    public static MessageReceiver Receiver => _receiver;
    public static MessageSender Sender => _sender;

    #endregion

    #region Private objects

    static MessageReceiver _receiver;
    static MessageSender _sender;

    #endregion

    #region Constructor

    static DeviceHandler()
    {
        _receiver = new MessageReceiver();
        _sender = new MessageSender();
    }

    #endregion
}
