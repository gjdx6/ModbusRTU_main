# NModbus 常用功能码

## 安装

在 NuGet 中安装稳定版：

```text
NModbus 3.0.83
NModbus.Serial 3.0.83
```

当前项目是 .NET 10。不要使用旧的 `NModbus4 2.1.0`，它会产生兼容性警告。

## 建立 RTU 主站

```csharp
using System.IO.Ports;
using NModbus;
using NModbus.Serial;

using var port = new SerialPort(
    "COM9",
    9600,
    Parity.None,
    8,
    StopBits.One);

port.ReadTimeout = 2000;
port.WriteTimeout = 2000;
port.Open();

var factory = new ModbusFactory();
using var master = factory.CreateRtuMaster(port);
```

`slaveAddress` 是从站地址，例如 `2`。`startAddress` 通常从 `0` 开始：手册中的 `40001` 通常对应程序地址 `0`。

## 功能码速查

| 功能码 | NModbus 方法 | 作用 | 数据类型 |
| --- | --- | --- | --- |
| 01 | `ReadCoils` | 读取输出线圈 | `bool[]` |
| 02 | `ReadInputs` | 读取离散输入 | `bool[]` |
| 03 | `ReadHoldingRegisters` | 读取保持寄存器 | `ushort[]` |
| 04 | `ReadInputRegisters` | 读取输入寄存器 | `ushort[]` |
| 05 | `WriteSingleCoil` | 写单个线圈 | `bool` |
| 06 | `WriteSingleRegister` | 写单个寄存器 | `ushort` |
| 0F | `WriteMultipleCoils` | 写多个线圈 | `bool[]` |
| 10 | `WriteMultipleRegisters` | 写多个寄存器 | `ushort[]` |

## 01 - 读取输出线圈

适合读取可读写的开关量，例如启动状态、继电器状态。

```csharp
bool[] coils = master.ReadCoils(
    slaveAddress: 2,
    startAddress: 0,
    numberOfPoints: 10);

bool firstCoil = coils[0];
```

对应 RTU 请求结构：

```text
02 01 00 00 00 0A CRC
```

## 02 - 读取离散输入

适合读取只读开关量，例如按钮、传感器输入、设备报警输入。

```csharp
bool[] inputs = master.ReadInputs(
    slaveAddress: 2,
    startAddress: 0,
    numberOfPoints: 8);
```

功能码 02 与 01 的区别是数据区不同：02 的数据通常不能写入。

## 03 - 读取保持寄存器

适合读取可读写的 16 位数值，例如设定值、速度、阈值。

```csharp
ushort[] registers = master.ReadHoldingRegisters(
    slaveAddress: 2,
    startAddress: 0,
    numberOfPoints: 2);

ushort firstValue = registers[0];
```

设备可能用两个寄存器表示一个 `int` 或 `float`。此时还要按设备手册处理高低字和字节序。

## 04 - 读取输入寄存器

适合读取只读 16 位数值，例如温度、电压、电流、采集量。

```csharp
ushort[] values = master.ReadInputRegisters(
    slaveAddress: 2,
    startAddress: 0,
    numberOfPoints: 2);
```

功能码 04 通常不能写入。

## 05 - 写单个线圈

写一个开关量，例如启动或停止一个设备。

```csharp
master.WriteSingleCoil(
    slaveAddress: 2,
    coilAddress: 0,
    value: true);
```

`true` 对应 Modbus 数据 `FF 00`，`false` 对应 `00 00`。

## 06 - 写单个寄存器

写一个 16 位保持寄存器。

```csharp
master.WriteSingleRegister(
    slaveAddress: 2,
    registerAddress: 0,
    value: 123);
```

`value` 是 `ushort`，范围是 `0` 到 `65535`。需要写负数时，按设备手册把 `short` 转为对应的 `ushort` 位模式。

## 0F - 写多个线圈

一次写多个开关量，数组第 0 项对应起始线圈地址。

```csharp
bool[] values = { true, false, true, true };

master.WriteMultipleCoils(
    slaveAddress: 2,
    startAddress: 0,
    data: values);
```

NModbus 会自动完成线圈位打包和 CRC，不需要手动拼报文。

## 10 - 写多个寄存器

一次写多个 16 位保持寄存器。

```csharp
ushort[] values = { 100, 200, 300 };

master.WriteMultipleRegisters(
    slaveAddress: 2,
    startAddress: 0,
    data: values);
```

常用于同时写入多个设定值。

## 常用异常处理

```csharp
try
{
    ushort[] values = master.ReadHoldingRegisters(2, 0, 2);
}
catch (TimeoutException)
{
    // 从站未响应：检查站号、串口参数、接线和设备状态。
}
catch (IOException)
{
    // 串口被占用、断开或底层 I/O 异常。
}
catch (SlaveException ex)
{
    // 从站返回 Modbus 异常码，例如地址非法、功能码不支持。
    Console.WriteLine(ex.Message);
}
```

## 实战顺序

1. 先用 Modbus Poll 或串口调试工具确认设备能通信。
2. 用功能码 01 或 03 做第一次 NModbus 读取。
3. 对比从站地址、地址偏移、数量和串口参数。
4. 再练习 05、06 写单个值。
5. 最后练习 0F、10 批量写入和浮点数的字节序转换。

## 容易混淆的点

- `01/02` 返回 `bool[]`，一个线圈就是一个布尔值。
- `03/04` 返回 `ushort[]`，每个寄存器固定是 16 位。
- `05/0F` 只能写输出线圈，不能写输入线圈。
- `06/10` 只能写保持寄存器，不能写输入寄存器。
- `40001`、`30001` 是手册显示地址，NModbus 通常传入从 `0` 开始的偏移地址。
- 设备手册永远优先：不是每台设备都支持所有功能码。
