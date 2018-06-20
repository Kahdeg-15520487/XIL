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
| `Int32` | currentInstruction | instruction pointer | 
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
| `Int32` | CurrentFStack | Current function stack | 
| `Int32` | InstructionCount | program length | 
| `Boolean` | IsDoneExecuting |  | 
| `Boolean` | IsLoaded |  | 
| `Boolean` | IsRunning |  | 
| `Boolean` | IsStackEmpty | Check if the stack is empty | 
| `Instruction` | Item | get an instruction at ``, will throw exception if using negative or out of range index | 
| `Int32` | StackTopIndex | stack's top index | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AppendInstruction(`Instruction` instr) |  | 
| `void` | Clear() | clear the stack | 
| `void` | EndExecution() | end this thread | 
| `Instruction` | FetchInstruction() | fetch the next instruction and advance the instruction pointer | 
| `Int32` | Get(`Int32` index) | Get the value of the stack element at index | 
| `String` | GetString(`Int32` index) |  | 
| `void` | LoadInstructions(`Instruction[]` instrs, `String[]` strs) |  | 
| `void` | PauseExecution() | pause this thread | 
| `Int32` | Peek() | peek the value on tots | 
| `Int32` | PeekF() | Peek the top function return on stack | 
| `Int32` | Pop() | Pop off tots and return it | 
| `Int32[]` | PopArray(`Int32` arrayIndex, `Int32` arraySize) |  | 
| `Int32` | PopF() | Pop a function return address from stack | 
| `Int32` | Push(`Int32` value) | push a value on tots | 
| `void` | PushArray(`Int32[]` array) | push an array on tots | 
| `void` | PushF(`Int32` f) | Push a function return address on stack | 
| `void` | RuntimeError(`String` errmsg) | raise a runtime error on this thread | 
| `void` | Set(`Int32` index, `Int32` value) | Set the stack element at index to the given value | 
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


