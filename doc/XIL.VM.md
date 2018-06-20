## `Priority`

```csharp
public enum XIL.VM.Priority
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Low |  | 
| `1` | Normal |  | 
| `2` | High |  | 
| `3` | Exclusive |  | 


## `Program`

```csharp
public class XIL.VM.Program

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32[]` | Bytecode |  | 
| `String[]` | StringTable |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Deserialize(`Stream` stream, `Instruction[]&` instructions, `String[]&` stringTable) |  | 
| `void` | Serialize(`Stream` stream, `Program` program) |  | 


## `Thread`

```csharp
public class XIL.VM.Thread

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | currentInstruction |  | 
| `Int32` | ExitCode |  | 
| `Int32` | FunctionReturn |  | 
| `Boolean` | IsRuntimeError |  | 
| `Priority` | priority |  | 
| `Int32` | ReturnJump |  | 
| `String` | RuntimeErrorMessage |  | 
| `Stack` | stack |  | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | CurrentFStack |  | 
| `Int32` | InstructionCount |  | 
| `Boolean` | IsDoneExecuting |  | 
| `Boolean` | IsLoaded |  | 
| `Boolean` | IsRunning |  | 
| `Boolean` | IsStackEmpty |  | 
| `Instruction` | Item |  | 
| `Int32` | StackTopIndex |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AppendInstruction(`Instruction` instr) |  | 
| `void` | Clear() |  | 
| `void` | EndExecution() |  | 
| `Instruction` | FetchInstruction() |  | 
| `Int32` | Get(`Int32` index) |  | 
| `String` | GetString(`Int32` index) |  | 
| `void` | LoadInstructions(`Instruction[]` instrs, `String[]` strs) |  | 
| `void` | PauseExecution() |  | 
| `Int32` | Peek() |  | 
| `Int32` | PeekF() |  | 
| `Int32` | Pop() |  | 
| `Int32[]` | PopArray(`Int32` arrayIndex, `Int32` arraySize) |  | 
| `Int32` | PopF() |  | 
| `Int32` | Push(`Int32` value) |  | 
| `void` | PushArray(`Int32[]` array) |  | 
| `void` | PushF(`Int32` f) |  | 
| `void` | RuntimeError(`String` errmsg) |  | 
| `void` | Set(`Int32` index, `Int32` value) |  | 
| `void` | SetInstruction(`Int32` index, `Instruction` instr) |  | 


## `VirtualMachine`

```csharp
public class XIL.VM.VirtualMachine

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Int32>` | exitcodes |  | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | CurrentStep |  | 
| `Int32` | LastStep |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | LoadProgram(`Instruction[]` instrs, `String[]` strs) |  | 
| `void` | Run() |  | 
| `void` | RuntimeError(`Instruction` instruction, `String` errormsg) |  | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Dictionary<Int32, InstructionAction>` | instructionMap |  | 
| `List<String>` | loadedLibrary |  | 
| `Random` | randomNumberGenerator |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | ContainLibrary(`String` libname) |  | 


