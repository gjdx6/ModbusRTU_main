using System.Globalization;
using System.IO.Ports;
using System.Text;
using thinger.DataConvertLib;
using thinger.ModbusRTULib;
using ModbusDataType = thinger.ModbusRTULib.DataType;

namespace thinger.ModbusProject;

public enum StoreArea
{
    输出线圈0x,
    输入线圈1x,
    输出寄存器4x,
    输入寄存器3x
}

public partial class FrmModbusRTU : Form
{
    private readonly ModbusRTU modbus = new();
    private bool isConnected;
    private DataFormat dataFormat = DataFormat.ABCD;

    public FrmModbusRTU()
    {
        InitializeComponent();
        InitParam();
    }

    private void InitParam()
    {
        string[] portList = SerialPort.GetPortNames();
        if (portList.Length > 0)
        {
            cmb_Port.Items.AddRange(portList);
            cmb_Port.SelectedIndex = 0;
        }

        cmb_BaudRate.Items.AddRange(new[] { "2400", "4800", "9600", "19200", "38400" });
        cmb_BaudRate.SelectedIndex = 2;
        cmb_Parity.DataSource = Enum.GetNames<Parity>();
        cmb_Parity.SelectedIndex = 0;
        cmb_StopBits.DataSource = Enum.GetNames<StopBits>();
        cmb_StopBits.SelectedIndex = 1;
        cmb_DataBits.Items.AddRange(new[] { "7", "8" });
        cmb_DataBits.SelectedIndex = 1;
        cmb_DataFormat.DataSource = Enum.GetNames<DataFormat>();
        cmb_DataFormat.SelectedIndex = 0;
        cmb_StoreArea.DataSource = Enum.GetNames<StoreArea>();
        cmb_StoreArea.SelectedIndex = 0;
        cmb_DataType.DataSource = Enum.GetNames<ModbusDataType>();
        cmb_DataType.SelectedIndex = 0;
        lst_Info.Columns[1].Width = Width - lst_Info.Columns[0].Width - 20;
    }

    private void AddLog(int level, string info)
    {
        ListViewItem item = new(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), level);
        item.SubItems.Add(info);
        lst_Info.Items.Insert(0, item);
    }

    private void btnConnect_Click(object sender, EventArgs e)
    {
        if (isConnected)
        {
            AddLog(1, "Modbus 已经连接");
            return;
        }

        if (string.IsNullOrWhiteSpace(cmb_Port.Text))
        {
            AddLog(1, "请选择串口");
            return;
        }

        try
        {
            Parity parity = Enum.Parse<Parity>(cmb_Parity.Text, true);
            StopBits stopBits = Enum.Parse<StopBits>(cmb_StopBits.Text, true);
            isConnected = modbus.Connect(cmb_Port.Text, int.Parse(cmb_BaudRate.Text), parity,
                int.Parse(cmb_DataBits.Text), stopBits);
            AddLog(isConnected ? 0 : 1, isConnected ? "Modbus 连接成功" : "Modbus 连接失败");
        }
        catch (Exception ex)
        {
            AddLog(1, $"连接参数错误: {ex.Message}");
        }
    }

    private void btn_DisConnect_Click(object sender, EventArgs e)
    {
        modbus.DisConnect();
        isConnected = false;
        AddLog(0, "Modbus 断开连接");
    }

    private void btnRead_Click(object sender, EventArgs e)
    {
        if (!TryReadParameters(out byte devId, out ushort start, out ushort length,
                out StoreArea area, out ModbusDataType type))
            return;

        dataFormat = Enum.Parse<DataFormat>(cmb_DataFormat.Text, true);

        try
        {
            if (type == ModbusDataType.Bool)
            {
                if (area is not (StoreArea.输出线圈0x or StoreArea.输入线圈1x))
                {
                    AddLog(1, "Bool 只能读取线圈存储区");
                    return;
                }

                byte[]? packed = area == StoreArea.输出线圈0x
                    ? modbus.ReadOutputColis(devId, start, length)
                    : modbus.ReadInputColis(devId, start, length);

                if (packed is null)
                {
                    AddLog(1, "读取失败: " + modbus.LastError);
                    return;
                }

                string values = string.Join(" ", Enumerable.Range(0, length)
                    .Select(i => ((packed[i / 8] >> (i % 8)) & 1) == 1 ? "1" : "0"));
                AddLog(0, $"读取成功: {values}");
                return;
            }

            int registers = GetRegisterCount(type, length);
            byte[]? raw = area == StoreArea.输出寄存器4x
                ? modbus.ReadOutPutRegisters(devId, start, (ushort)registers)
                : area == StoreArea.输入寄存器3x
                    ? modbus.ReadInPutRegisters(devId, start, (ushort)registers)
                    : null;

            if (raw is null)
            {
                AddLog(1, "读取失败: " + (area is StoreArea.输出线圈0x or StoreArea.输入线圈1x
                    ? "当前数据类型需要寄存器存储区"
                    : modbus.LastError));
                return;
            }

            AddLog(0, "读取成功: " + DecodeValues(raw, type, length, dataFormat));
        }
        catch (Exception ex)
        {
            AddLog(1, "读取失败: " + ex.Message);
        }
    }

    private void btnWrite_Click(object sender, EventArgs e)
    {
        if (!TryReadParameters(out byte devId, out ushort start, out ushort length,
                out StoreArea area, out ModbusDataType type))
            return;

        dataFormat = Enum.Parse<DataFormat>(cmb_DataFormat.Text, true);

        try
        {
            if (type == ModbusDataType.Bool)
            {
                if (area != StoreArea.输出线圈0x)
                {
                    AddLog(1, "Bool 写入只能使用输出线圈 0x");
                    return;
                }

                bool[] values = ParseBoolValues(txt_WriteValue.Text);
                if (values.Length != length)
                {
                    AddLog(1, $"Bool 数量为 {values.Length}，读取长度应为 {values.Length}");
                    return;
                }

                bool success = values.Length == 1
                    ? modbus.PreSetSingleCoil(devId, start, values[0])
                    : modbus.PreSetMutiCoils(devId, start, values);
                AddLog(success ? 0 : 1, success ? "线圈写入成功" : "线圈写入失败: " + modbus.LastError);
                return;
            }

            if (area != StoreArea.输出寄存器4x)
            {
                AddLog(1, "寄存器写入只能使用保持寄存器 4x");
                return;
            }

            byte[] bytes = EncodeValues(txt_WriteValue.Text, type, dataFormat);
            int expectedRegisters = GetRegisterCount(type, length);
            if (bytes.Length > expectedRegisters * 2)
            {
                AddLog(1, "写入数据超过指定长度");
                return;
            }

            if (bytes.Length % 2 != 0)
                Array.Resize(ref bytes, bytes.Length + 1);

            bool registerSuccess = bytes.Length == 2
                ? modbus.PreSetSingleRegister(devId, start, bytes)
                : modbus.PreSetMutiRegisters(devId, start, bytes);
            AddLog(registerSuccess ? 0 : 1,
                registerSuccess ? "寄存器写入成功" : "寄存器写入失败: " + modbus.LastError);
        }
        catch (Exception ex)
        {
            AddLog(1, "写入失败: " + ex.Message);
        }
    }

    private bool TryReadParameters(out byte devId, out ushort start, out ushort length,
        out StoreArea area, out ModbusDataType type)
    {
        devId = 0;
        start = 0;
        length = 0;
        area = default;
        type = default;

        if (!isConnected)
        {
            AddLog(1, "请先连接 Modbus");
            return false;
        }

        if (!byte.TryParse(txt_SlaveID.Text, out devId) || devId is 0 or > 247)
        {
            AddLog(1, "站地址应为 1 到 247");
            return false;
        }

        if (!ushort.TryParse(txt_Start.Text, out start))
        {
            AddLog(1, "起始地址格式不正确");
            return false;
        }

        if (!ushort.TryParse(txt_Length.Text, out length) || length == 0)
        {
            AddLog(1, "长度应为大于 0 的数字");
            return false;
        }

        area = Enum.Parse<StoreArea>(cmb_StoreArea.Text, true);
        type = Enum.Parse<ModbusDataType>(cmb_DataType.Text, true);
        return true;
    }

    private static int GetRegisterCount(ModbusDataType type, int valueCount)
    {
        int bytesPerValue = type switch
        {
            ModbusDataType.Byte or ModbusDataType.ByteArray or ModbusDataType.HexString or ModbusDataType.String => 1,
            ModbusDataType.Short or ModbusDataType.UShort => 2,
            ModbusDataType.Int or ModbusDataType.UInt or ModbusDataType.Float => 4,
            ModbusDataType.Double or ModbusDataType.Long or ModbusDataType.ULong => 8,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        return (bytesPerValue * valueCount + 1) / 2;
    }

    private static string DecodeValues(byte[] raw, ModbusDataType type, int count, DataFormat format)
    {
        if (type == ModbusDataType.ByteArray || type == ModbusDataType.HexString)
            return ModbusValueConverter.ToHex(raw.Take(count).ToArray());

        if (type == ModbusDataType.String)
            return Encoding.UTF8.GetString(raw.Take(count).ToArray()).TrimEnd('\0');

        int width = type == ModbusDataType.Byte ? 1 : GetRegisterCount(type, 1) * 2;
        List<string> values = new();
        for (int offset = 0; offset + width <= raw.Length && values.Count < count; offset += width)
        {
            byte[] value = raw.Skip(offset).Take(width).ToArray();
            string text = type switch
            {
                ModbusDataType.Byte => value[0].ToString(CultureInfo.InvariantCulture),
                ModbusDataType.Short => ModbusValueConverter.ToInt16(value, format).ToString(CultureInfo.InvariantCulture),
                ModbusDataType.UShort => ModbusValueConverter.ToUInt16(value, format).ToString(CultureInfo.InvariantCulture),
                ModbusDataType.Int => ModbusValueConverter.ToInt32(value, format).ToString(CultureInfo.InvariantCulture),
                ModbusDataType.UInt => ModbusValueConverter.ToUInt32(value, format).ToString(CultureInfo.InvariantCulture),
                ModbusDataType.Float => ModbusValueConverter.ToSingle(value, format).ToString(CultureInfo.InvariantCulture),
                ModbusDataType.Double => ModbusValueConverter.ToDouble(value, format).ToString(CultureInfo.InvariantCulture),
                ModbusDataType.Long => ModbusValueConverter.ToInt64(value, format).ToString(CultureInfo.InvariantCulture),
                ModbusDataType.ULong => ModbusValueConverter.ToUInt64(value, format).ToString(CultureInfo.InvariantCulture),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            values.Add(text);
        }

        return string.Join(" ", values);
    }

    private static bool[] ParseBoolValues(string text)
    {
        string[] tokens = text.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        return tokens.Select(token => token.ToLowerInvariant() switch
        {
            "1" or "true" or "on" => true,
            "0" or "false" or "off" => false,
            _ => throw new FormatException($"无效的 Bool 值: {token}")
        }).ToArray();
    }

    private static byte[] EncodeValues(string text, ModbusDataType type, DataFormat format)
    {
        string[] tokens = text.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        List<byte> bytes = new();

        switch (type)
        {
            case ModbusDataType.Byte:
                foreach (string token in tokens)
                    bytes.Add(ParseByte(token));
                break;
            case ModbusDataType.ByteArray:
            case ModbusDataType.HexString:
                return ModbusValueConverter.ParseHex(text);
            case ModbusDataType.String:
                return Encoding.UTF8.GetBytes(text);
            case ModbusDataType.Short:
                foreach (string token in tokens)
                    bytes.AddRange(ModbusValueConverter.FromInt16(short.Parse(token, CultureInfo.InvariantCulture), format));
                break;
            case ModbusDataType.UShort:
                foreach (string token in tokens)
                    bytes.AddRange(ModbusValueConverter.FromUInt16(ushort.Parse(token, CultureInfo.InvariantCulture), format));
                break;
            case ModbusDataType.Int:
                foreach (string token in tokens)
                    bytes.AddRange(ModbusValueConverter.FromInt32(int.Parse(token, CultureInfo.InvariantCulture), format));
                break;
            case ModbusDataType.UInt:
                foreach (string token in tokens)
                    bytes.AddRange(ModbusValueConverter.FromUInt32(uint.Parse(token, CultureInfo.InvariantCulture), format));
                break;
            case ModbusDataType.Float:
                foreach (string token in tokens)
                    bytes.AddRange(ModbusValueConverter.FromSingle(float.Parse(token, CultureInfo.InvariantCulture), format));
                break;
            case ModbusDataType.Double:
                foreach (string token in tokens)
                    bytes.AddRange(ModbusValueConverter.FromDouble(double.Parse(token, CultureInfo.InvariantCulture), format));
                break;
            case ModbusDataType.Long:
                foreach (string token in tokens)
                    bytes.AddRange(ModbusValueConverter.FromInt64(long.Parse(token, CultureInfo.InvariantCulture), format));
                break;
            case ModbusDataType.ULong:
                foreach (string token in tokens)
                    bytes.AddRange(ModbusValueConverter.FromUInt64(ulong.Parse(token, CultureInfo.InvariantCulture), format));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return bytes.ToArray();
    }

    private static byte ParseByte(string token)
    {
        if (token.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return Convert.ToByte(token[2..], 16);

        return byte.Parse(token, CultureInfo.InvariantCulture);
    }
}
