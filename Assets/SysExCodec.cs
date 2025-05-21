using System;

public static class SysExCodec
{
    public static Span<byte> EncodeTo7Bit
      (ReadOnlySpan<byte> source, Span<byte> buffer)
    {
        var len = (source.Length + 6) / 7 * 8;

        if (buffer.Length < len)
            throw new ArgumentException("Destination buffer is too short");

        for (var (src, dst) = (0, 0); src < source.Length;)
        {
            var msb = 0;
            for (var i = 0; i < 7; i++)
            {
                var b = source[src + i];
                buffer[dst + i + 1] = (byte)(b & 0x7f);
                if ((b & 0x80) != 0) msb |= 1 << i;
            }
            buffer[dst] = (byte)msb;
            (src, dst) = (src + 7, dst + 8);
        }

        return buffer.Slice(0, len);
    }

    public static Span<byte> DecodeTo8Bit
      (ReadOnlySpan<byte> source, Span<byte> buffer)
    {
        var len = (source.Length + 7) / 8 * 7;

        if (buffer.Length < len)
            throw new ArgumentException("Destination buffer is too short");

        for (var (src, dst) = (0, 0); src < source.Length;)
        {
            var msb = source[src];
            for (var i = 0; i < 7; i++)
            {
                var b = source[src + i + 1];
                if (((msb >> i) & 1) != 0) b |= 0x80;
                buffer[dst + i] = (byte)b;
            }
            (src, dst) = (src + 8, dst + 7);
        }

        return buffer.Slice(0, len);
    }
}









