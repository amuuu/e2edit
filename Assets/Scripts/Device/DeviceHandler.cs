using UnityEngine;

public sealed class DeviceHandler : MonoBehaviour
{
    #region Public accessors

    public MessageReceiver Receiver => _receiver;
    public MessageSender Sender => _sender;

    #endregion

    #region Private objects

    MessageReceiver _receiver;
    MessageSender _sender;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _receiver = new MessageReceiver();
        _sender = new MessageSender();
    }

    void OnDestroy()
    {
        _sender?.Dispose();
        _receiver?.Dispose();
        (_sender, _receiver) = (null, null);
    }

    #endregion
}
