# Modbus Master Completion Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Complete the existing WinForms Modbus RTU master data conversion, write controls, error handling, and repeatable verification.

**Architecture:** Keep serial framing and Modbus function-code methods in `thinger.ModbusRTULib`. Keep UI parsing, validation, and display in `FrmModbusRTU`. Add a small conversion utility with no serial dependency so its behavior can be tested independently.

**Tech Stack:** C#/.NET 10, WinForms, `System.IO.Ports`, existing CRC16 implementation, `dotnet build`; use Modbus Slave for manual RTU verification.

---

### Task 1: Establish a clean baseline

**Files:**
- Modify: `thinger.ModbusRTULib/ModbusRTU.cs`
- Modify: `thinger.ModbusProject/FrmModbusRTU.cs`

- [ ] **Step 1: Resolve the `DataType` ambiguity.**

In `FrmModbusRTU.cs`, replace unqualified application enum references with an alias:

```csharp
using ModbusDataType = thinger.ModbusRTULib.DataType;
```

Use `ModbusDataType` for `Enum.GetNames`, `Enum.Parse`, and all switch cases. Do not use `System.ComponentModel.DataAnnotations.DataType`.

- [ ] **Step 2: Build the baseline.**

Run:

```powershell
dotnet build "D:\c#-project\modbus-project\ModbusRTU_Main\thinger.ModbusProject\thinger.ModbusProject.csproj" --nologo
```

Expected: the `DataType` ambiguity errors are gone; existing nullable warnings may remain temporarily.

### Task 2: Add testable conversion helpers

**Files:**
- Create: `thinger.ModbusRTULib/ModbusValueConverter.cs`
- Create: `tests/thinger.ModbusRTULib.Tests/thinger.ModbusRTULib.Tests.csproj`
- Create: `tests/thinger.ModbusRTULib.Tests/ModbusValueConverterTests.cs`

- [ ] **Step 1: Add failing tests for byte order and numeric conversion.**

The tests must cover these expected mappings:

```csharp
Assert.Equal(new byte[] { 0x12, 0x34, 0x56, 0x78 },
    ModbusValueConverter.ApplyFormat(new byte[] { 0x12, 0x34, 0x56, 0x78 }, DataFormat.ABCD));
Assert.Equal(new byte[] { 0x34, 0x12, 0x78, 0x56 },
    ModbusValueConverter.ApplyFormat(new byte[] { 0x12, 0x34, 0x56, 0x78 }, DataFormat.BADC));
Assert.Equal(new byte[] { 0x56, 0x78, 0x12, 0x34 },
    ModbusValueConverter.ApplyFormat(new byte[] { 0x12, 0x34, 0x56, 0x78 }, DataFormat.CDAB));
Assert.Equal(new byte[] { 0x78, 0x56, 0x34, 0x12 },
    ModbusValueConverter.ApplyFormat(new byte[] { 0x12, 0x34, 0x56, 0x78 }, DataFormat.DCBA));
```

Also test invalid byte lengths, signed/unsigned 16-bit values, 32-bit integers,
single-precision floats, and invalid text input.

- [ ] **Step 2: Run the tests and confirm they fail for the missing converter.**

Run:

```powershell
dotnet test "D:\c#-project\modbus-project\ModbusRTU_Main\tests\thinger.ModbusRTULib.Tests\thinger.ModbusRTULib.Tests.csproj" --nologo
```

Expected: compilation fails because `ModbusValueConverter` does not exist yet.

- [ ] **Step 3: Implement the minimal converter.**

Provide methods for `ApplyFormat`, `ToInt16`, `ToUInt16`, `ToInt32`, `ToUInt32`,
`ToSingle`, `ToDouble`, `ToInt64`, `ToUInt64`, and hexadecimal formatting. Reject
incorrect byte lengths with `ArgumentException` and invalid text with
`FormatException`.

- [ ] **Step 4: Run the converter tests.**

Expected: all converter tests pass.

### Task 3: Harden the Modbus RTU library

**Files:**
- Modify: `thinger.ModbusRTULib/ModbusRTU.cs`
- Modify: `tests/thinger.ModbusRTULib.Tests/ModbusFrameTests.cs`

- [ ] **Step 1: Add failing frame tests.**

Test known CRC frames, response length checks, byte-count checks, and rejection
of the wrong slave address or function code.

- [ ] **Step 2: Fix the existing implementation.**

Preserve the public methods for function codes 01, 02, 03, 04, 05, 06, 0F, and
10. Ensure function 0F includes packed coil data before CRC, function 10 uses
register count rather than byte count, and every response validates CRC, address,
function, length, and byte count before parsing.

Make the connection state explicit, reject calls while disconnected, and retain
the last communication error for the UI instead of swallowing every exception.

- [ ] **Step 3: Run unit tests and build both projects.**

Expected: tests pass and `dotnet build` exits with code 0.

### Task 4: Complete read display behavior

**Files:**
- Modify: `thinger.ModbusProject/FrmModbusRTU.cs`

- [ ] **Step 1: Add parsing tests for the UI-facing conversion path.**

Verify that the selected store area maps to function 01/02/03/04, coil data is
shown as boolean values, and register data uses the selected data type and
`DataFormat`.

- [ ] **Step 2: Implement read display.**

Read the selected station, address, quantity, data type, and byte format from the
controls. Display one result per requested value, show hexadecimal bytes for
`HexString`/`ByteArray`, and show a clear validation error for unsupported sizes.

- [ ] **Step 3: Verify with Modbus Slave.**

Read coils, input status, holding registers, and input registers using known test
values and compare the displayed values with the simulator.

### Task 5: Complete write controls

**Files:**
- Modify: `thinger.ModbusProject/FrmModbusRTU.cs`
- Modify: `thinger.ModbusProject/FrmModbusRTU.Designer.cs` only if controls are missing

- [ ] **Step 1: Add input validation tests.**

Cover empty input, invalid numeric text, invalid hex text, coil values other than
0/1 or true/false, and register quantities that do not match the supplied data.

- [ ] **Step 2: Implement write dispatch.**

Use function 05 for one coil, 06 for one register, 0F for multiple coils, and 10
for multiple registers. Convert values using the selected data type and byte
format. Show success only after the library confirms the response.

- [ ] **Step 3: Verify writes with Modbus Slave.**

Write one and multiple coils, then read them back. Write one and multiple
registers, then read them back and confirm the selected byte order.

### Task 6: Final verification

**Files:**
- Modify: `thinger.ModbusRTULib/ModbusRTU.cs` only for verified defects found by tests

- [ ] **Step 1: Run the full test suite.**

```powershell
dotnet test "D:\c#-project\modbus-project\ModbusRTU_Main\tests\thinger.ModbusRTULib.Tests\thinger.ModbusRTULib.Tests.csproj" --nologo
```

- [ ] **Step 2: Build the application.**

```powershell
dotnet build "D:\c#-project\modbus-project\ModbusRTU_Main\thinger.ModbusProject\thinger.ModbusProject.csproj" --nologo
```

- [ ] **Step 3: Review the diff.**

Confirm that only source, tests, and project files changed; no `bin`, `obj`, or
`.vs` files are staged.
