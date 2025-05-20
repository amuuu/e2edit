using System;

public class SysExDataBuffer
{
    readonly byte[] _buffer;
    bool _isEncoded;

    public int DecodedLength => _buffer.Length / 8 * 7;
    public int EncodedLength => _buffer.Length;

    public bool IsEncoded => _isEncoded;
    public int Length => _isEncoded ? EncodedLength : DecodedLength;
    public Span<byte> Buffer =>
      _buffer.AsSpan(_isEncoded ? 0 : _buffer.Length - Length, Length);

    public static SysExDataBuffer Create7Bit(int length)
    {
        if (length % 8 != 0)
            throw new ArgumentException("Length must be a multiple of 8");
        return new SysExDataBuffer(new byte[length], true);
    }

    public static SysExDataBuffer Create8Bit(int length)
    {
        if (length % 7 != 0)
            throw new ArgumentException("Length must be a multiple of 7");
        return new SysExDataBuffer(new byte[length / 7 * 8], false);
    }

    SysExDataBuffer(byte[] buffer, bool isEncoded)
      => (_buffer, _isEncoded) = (buffer, isEncoded);

    public void EncodeTo7Bit()
    {
        if (_isEncoded) return;

        var temp = (Span<byte>)(stackalloc byte[8]);
        var src = EncodedLength - DecodedLength;
        var dst = 0;

        for (var i = 0; i < DecodedLength; i += 7)
        {
            var msb = 0;
            for (var j = 0; j < 7; j++)
            {
                var b = _buffer[src + i + j];
                temp[j + 1] = (byte)(b & 0x7F);
                if ((b & 0x80) != 0) msb |= 1 << j;
            }
            temp[0] = (byte)msb;
            temp.CopyTo(_buffer.AsSpan(dst));
            dst += 8;
        }

        _isEncoded = true;
    }

    public void DecodeTo8Bit()
    {
        if (!_isEncoded) return;

        var temp = (Span<byte>)(stackalloc byte[8]);
        var src = EncodedLength - 8;
        var dst = EncodedLength - 7;

        while (src >= 0)
        {
            _buffer.AsSpan(src, 8).CopyTo(temp);
            var msb = temp[0];
            for (var i = 0; i < 7; i++)
            {
                var b = temp[i + 1];
                if (((msb >> i) & 1) != 0) b |= 0x80;
                _buffer[dst + i] = (byte)b;
            }
            src -= 8;
            dst -= 7;
        }

        _isEncoded = false;
    }
}
