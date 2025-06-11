static class MidiPortProbe
{
    public static RtMidi.MidiIn OpenInPort()
    {
        var port = RtMidi.MidiIn.Create();
        for (var i = 0; i < port.PortCount; i++)
        {
            if (port.GetPortName(i).StartsWith("electribe2"))
            {
                port.OpenPort(i);
                port.IgnoreTypes(sysex: false);
                return port;
            }
        }
        port.Dispose();
        return null;
    }

    public static RtMidi.MidiOut OpenOutPort()
    {
        var port = RtMidi.MidiOut.Create();
        for (var i = 0; i < port.PortCount; i++)
        {
            if (port.GetPortName(i).StartsWith("electribe2"))
            {
                port.OpenPort(i);
                return port;
            }
        }
        port.Dispose();
        return null;
    }
}
