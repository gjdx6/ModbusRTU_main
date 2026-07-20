# Modbus Master Completion Design

## Scope

Complete the existing WinForms Modbus RTU master in three areas:

- Resolve the `DataType` naming conflict and existing compiler issues.
- Convert register and coil data into readable values using the selected data type and byte order.
- Add write controls for single/multiple coils and registers.

## Design

Keep `thinger.ModbusRTULib` responsible for Modbus RTU framing, CRC, serial I/O,
and response validation. Keep `FrmModbusRTU` responsible for user input, display,
and selecting the appropriate library method.

Use explicit type names where the application enum conflicts with framework types:

```csharp
thinger.ModbusRTULib.DataType
```

Add focused conversion helpers for `Bool`, signed/unsigned integers, floating
point values, strings, raw bytes, and hexadecimal text. Conversion helpers will
apply the selected `DataFormat` byte order only to multi-byte values.

Write operations will validate station ID, address, quantity, data type, and input
text before sending. The UI will select function code 05, 06, 0F, or 10 according
to the data area and quantity, then report success or the actual communication
failure.

The library will retain its current public API where practical. Existing response
checks will be tightened for address, function code, byte count, response length,
and CRC. Communication exceptions will be surfaced through a clear result or
diagnostic message instead of being silently discarded.

## Verification

Add tests for code that does not require a physical serial port:

- Modbus CRC known frames.
- Boolean packing and unpacking, including non-multiples of eight.
- ABCD, BADC, CDAB, and DCBA ordering.
- Numeric conversion and invalid input handling.
- Request/response length and CRC validation.

Use Modbus Slave or a virtual serial pair for manual RTU verification of read and
write operations after the unit tests pass.

## Out Of Scope

This iteration will not add uncommon diagnostic functions such as 07, 08, 0B,
0C, 11, 12, 17, 22, or 24. It will not replace the current serial transport
with NModbus or redesign the entire UI.
