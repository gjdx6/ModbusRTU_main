using thinger.DataConvertLib;
using thinger.ModbusRTULib;
using Xunit;

namespace thinger.ModbusRTULib.Tests;

public class ModbusValueConverterTests
{
    [Theory]
    [InlineData(DataFormat.ABCD, new byte[] { 0x12, 0x34, 0x56, 0x78 })]
    [InlineData(DataFormat.BADC, new byte[] { 0x34, 0x12, 0x78, 0x56 })]
    [InlineData(DataFormat.CDAB, new byte[] { 0x56, 0x78, 0x12, 0x34 })]
    [InlineData(DataFormat.DCBA, new byte[] { 0x78, 0x56, 0x34, 0x12 })]
    public void ApplyFormatReordersFourBytes(DataFormat format, byte[] expected)
    {
        byte[] actual = ModbusValueConverter.ApplyFormat(
            new byte[] { 0x12, 0x34, 0x56, 0x78 }, format);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ApplyFormatReordersTwoBytes()
    {
        Assert.Equal(
            new byte[] { 0x34, 0x12 },
            ModbusValueConverter.ApplyFormat(
                new byte[] { 0x12, 0x34 }, DataFormat.BADC));
    }

    [Fact]
    public void ToInt16UsesBigEndianRegisterBytes()
    {
        Assert.Equal(
            4660,
            ModbusValueConverter.ToInt16(new byte[] { 0x12, 0x34 }, DataFormat.ABCD));
    }

    [Fact]
    public void ToSingleUsesSelectedByteOrder()
    {
        byte[] ieee754 = BitConverter.GetBytes(1.0f);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(ieee754);

        Assert.Equal(
            1.0f,
            ModbusValueConverter.ToSingle(ieee754, DataFormat.ABCD),
            precision: 5);
    }

    [Fact]
    public void ApplyFormatRejectsIncompleteMultiByteValue()
    {
        Assert.Throws<ArgumentException>(() =>
            ModbusValueConverter.ApplyFormat(new byte[] { 0x01, 0x02, 0x03 }, DataFormat.CDAB));
    }

    [Fact]
    public void ParseHexRejectsOddLength()
    {
        Assert.Throws<FormatException>(() =>
            ModbusValueConverter.ParseHex("0A1"));
    }
}
