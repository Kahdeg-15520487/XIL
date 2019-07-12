## `Priority`

Thread's priority
```csharp
public enum XIL.VM.Priority
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `1` | Low | 1 timeslice | 
| `4` | Normal | 4 timeslice | 
| `8` | High | 8 timeslice | 
| `2147483647` | Exclusive | run till done | 


## `Program`

An in-memory program
```csharp
public class XIL.VM.Program

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32[]` | Bytecode | bytecodes | 
| `String[]` | StringTable | string constant table | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Deserialize(`Stream` stream, `Instruction[]&` instructions, `String[]&` stringTable) | serialize a program from a stream | 
| `void` | Serialize(`Stream` stream, `Program` program) | serialize a program into a stream for saving | 


## `Thread`

An execution thread
```csharp
public class XIL.VM.Thread

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | currentInstruction | instruction pointer | 
| `Int32` | ExitCode | exit code | 
| `Int32` | FunctionReturn | where to return function | 
| `Boolean` | IsRuntimeError | does the thread has a runtime error | 
| `Priority` | Priority | thread's priority | 
| `Int32` | ReturnJump | where to return jump | 
| `String` | RuntimeErrorMessage | runtime error's message | 
| `Stack` | stack |  | 
| `ThreadState` | State | thread's state | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | CurrentFStack | Current function stack | 
| `Int32` | InstructionCount | program length | 
| `Boolean` | IsDoneExecuting | has the thread done running | 
| `Boolean` | IsLoaded | has the thread been loaded with a program | 
| `Boolean` | IsRunning | is the thread running | 
| `Boolean` | IsStackEmpty | Check if the stack is empty | 
| `Instruction` | Item | get an instruction at ``, will throw exception if using negative or out of range index | 
| `Int32` | StackTopIndex | stack's top index | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AppendInstruction(`Instruction` instr) | append an instruction into the thread  Has not been implemented  for runtime modifying capability | 
| `void` | Clear() | clear the stack | 
| `void` | EndExecution() | end this thread | 
| `Instruction` | FetchInstruction() | fetch the next instruction and advance the instruction pointer | 
| `Int32` | Get(`Int32` index) | Get the value of the stack element at index | 
| `String` | GetString(`Int32` index) | get a string from the thread's string table | 
| `void` | LoadInstructions(`Instruction[]` instrs, `String[]` strs) | load a program into a thread | 
| `void` | PauseExecution() | pause this thread | 
| `Int32` | Peek() | peek the value on tots | 
| `Int32` | PeekF() | Peek the top function return on stack | 
| `Int32` | Pop() | Pop off tots and return it | 
| `Int32[]` | PopArray(`Int32` arrayIndex, `Int32` arraySize) | pop an array from tots | 
| `Int32` | PopF() | Pop a function return address from stack | 
| `Int32` | Push(`Int32` value) | push a value on tots | 
| `void` | PushArray(`Int32[]` array) | push an array on tots | 
| `void` | PushF(`Int32` f) | Push a function return address on stack | 
| `void` | RuntimeError(`String` errmsg) | raise a runtime error on this thread | 
| `void` | Set(`Int32` index, `Int32` value) | Set the stack element at index to the given value | 
| `void` | SetInstruction(`Int32` index, `Instruction` instr) | modify an instruction  for runtime modifying capability | 


## `ThreadState`

Thread's state
```csharp
public enum XIL.VM.ThreadState
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Running | thread is running | 
| `1` | Pause | thread is paused | 
| `2` | Done | thread has done executing | 


## `VirtualMachine`

A XIL virtual machine
```csharp
public class XIL.VM.VirtualMachine

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Int32>` | Exitcodes | exitcode of all thread | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `VirtualMachineVerboseLevel` | VerboseLevel | log verbosity level | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | LoadProgram(`Instruction[]` instrs, `String[]` strs) | load a program | 
| `void` | Run() | run the virtual machine synchronously | 
| `Int32` | Tick() | tick an update | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Dictionary<Int32, InstructionAction>` | InstructionMap | a list of instruction's implementation | 
| `List<String>` | LoadedLibrary | a list of loaded instruction library's name | 
| `Random` | RandomNumberGenerator | random number generator | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | ContainLibrary(`String` libname) | check if a library has been loaded | 


## `VirtualMachineVerboseLevel`

```csharp
public enum XIL.VM.VirtualMachineVerboseLevel
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | None | log nothing | 
| `1` | LoadtimeError | log loadtime error | 
| `2` | RuntimeError | log runtime error | 
| `4` | InstructionInfo | log instruction detail | 
| `8` | ThreadInfo | log thead information | 


