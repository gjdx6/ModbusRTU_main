using System.Buffers.Binary;

using thinger.DataConvertLib;

namespace thinger.ModbusRTULib;

public static class ModbusValueConverter
{
    public static byte[] ApplyFormat(byte[] value, DataFormat format)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value.Length == 0 || value.Length == 1)
            return value.ToArray();

        if (value.Length != 2 && value.Length != 4 && value.Length != 8)
            throw new ArgumentException("Only 2, 4, or 8 byte values are supported.", nameof(value));

        byte[] result = value.ToArray();

        if (value.Length == 2)
        {
            if (format is DataFormat.BADC or DataFormat.DCBA)
                Array.Reverse(result);

            return result;
        }

        for (int offset = 0; offset < result.Length; offset += 4)
        {
            byte[] word = result.AsSpan(offset, 4).ToArray();

            switch (format)
            {
                case DataFormat.ABCD:
                    break;
                case DataFormat.BADC:
                    (word[0], word[1]) = (word[1], word[0]);
                    (word[2], word[3]) = (word[3], word[2]);
                    break;
                case DataFormat.CDAB:
                    (word[0], word[1], word[2], word[3]) =
                        (word[2], word[3], word[0], word[1]);
                    break;
                case DataFormat.DCBA:
                    Array.Reverse(word);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }

            word.CopyTo(result, offset);
        }

        return result;
    }

    public static short ToInt16(byte[] value, DataFormat format) =>
        BinaryPrimitives.ReadInt16BigEndian(ApplyFormat(value, format));

    public static ushort ToUInt16(byte[] value, DataFormat format) =>
        BinaryPrimitives.ReadUInt16BigEndian(ApplyFormat(value, format));

    public static int ToInt32(byte[] value, DataFormat format) =>
        BinaryPrimitives.ReadInt32BigEndian(ApplyFormat(value, format));

    public static uint ToUInt32(byte[] value, DataFormat format) =>
        BinaryPrimitives.ReadUInt32BigEndian(ApplyFormat(value, format));

    public static long ToInt64(byte[] value, DataFormat format) =>
        BinaryPrimitives.ReadInt64BigEndian(ApplyFormat(value, format));

    public static ulong ToUInt64(byte[] value, DataFormat format) =>
        BinaryPrimitives.ReadUInt64BigEndian(ApplyFormat(value, format));

    public static float ToSingle(byte[] value, DataFormat format)
    {
        int bits = BinaryPrimitives.ReadInt32BigEndian(ApplyFormat(value, format));
        return BitConverter.Int32BitsToSingle(bits);
    }

    public static double ToDouble(byte[] value, DataFormat format)
    {
        long bits = BinaryPrimitives.ReadInt64BigEndian(ApplyFormat(value, format));
        return BitConverter.Int64BitsToDouble(bits);
    }

    public static byte[] FromInt16(short value, DataFormat format)
    {
        byte[] bytes = new byte[2];
        BinaryPrimitives.WriteInt16BigEndian(bytes, value);
        return ApplyFormat(bytes, format);
    }

    public static byte[] FromUInt16(ushort value, DataFormat format)
    {
        byte[] bytes = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(bytes, value);
        return ApplyFormat(bytes, format);
    }

    public static byte[] FromInt32(int value, DataFormat format)
    {
        byte[] bytes = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(bytes, value);
        return ApplyFormat(bytes, format);
    }

    public static byte[] FromUInt32(uint value, DataFormat format)
    {
        byte[] bytes = new byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(bytes, value);
        return ApplyFormat(bytes, format);
    }

    public static byte[] FromInt64(long value, DataFormat format)
    {
        byte[] bytes = new byte[8];
        BinaryPrimitives.WriteInt64BigEndian(bytes, value);
        return ApplyFormat(bytes, format);
    }

    public static byte[] FromUInt64(ulong value, DataFormat format)
    {
        byte[] bytes = new byte[8];
        BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
        return ApplyFormat(bytes, format);
    }

    public static byte[] FromSingle(float value, DataFormat format) =>
        FromInt32(BitConverter.SingleToInt32Bits(value), format);

    public static byte[] FromDouble(double value, DataFormat format) =>
        FromInt64(BitConverter.DoubleToInt64Bits(value), format);

    public static byte[] ParseHex(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        string normalized = text
            .Replace(" ", string.Empty)
            .Replace("-", string.Empty)
            .Replace(",", string.Empty);

        if (normalized.Length % 2 != 0)
            throw new FormatException("Hexadecimal text must contain an even number of digits.");

        byte[] result = new byte[normalized.Length / 2];
        for (int i = 0; i < result.Length; i++)
        {
            if (!byte.TryParse(normalized.AsSpan(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber,
                    null,
                    out result[i]))
            {
                throw new FormatException($"Invalid hexadecimal value at position {i * 2}.");
            }
        }

        return result;
    }

    public static string ToHex(byte[] value) =>
        string.Join(" ", value.Select(static b => b.ToString("X2")));
}
