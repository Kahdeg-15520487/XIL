## `Assembler`

XIL Assembler
```csharp
public class XIL.Assembler.Assembler

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `CompileResult` | Compile(`String` sourcecode) | compile a xil script | 
| `CompileResult` | Compile(`String` sourcecode, `ICodeGenerator` codegen) | compile a xil script | 


## `CodeGenerator`

codegen
```csharp
public class XIL.Assembler.CodeGenerator
    : ICodeGenerator

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AddInstruction(`Instruction` instruction) | add an instruction | 
| `void` | AddInstruction(`Int32` opcode, `Int32` op1 = 0, `Int32` op2 = 0, `Int32` lnb = 0) | add an instruction | 
| `void` | AddJumpLabel(`String` label, `Int32` linecount) | add a jump label | 
| `Int32` | AddLibrary(`String` lib) | add a library metadata | 
| `Int32` | AddString(`String` str) | add a string constant  return the index of the added string constant | 
| `Program` | Emit() | emit a program | 
| `Int32` | GetJumpLabel(`String` label) | get a jump label's target | 
| `Int32` | GetLibrary(`String` lib) | get a library index | 
| `Int32` | GetString(`String` str) | retrieve a string constant  return -1 if string constant is not exist | 


## `CompileResult`

result for a compilation
```csharp
public struct XIL.Assembler.CompileResult

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `ICodeGenerator` | CodeGenerator | the code generator, to be replaced with an interface | 
| `String` | Message | error message | 
| `Boolean` | Success | success? | 


## `ICodeGenerator`

ICodeGenerator interface
```csharp
public interface XIL.Assembler.ICodeGenerator

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AddInstruction(`Instruction` instruction) | add an instruction | 
| `void` | AddInstruction(`Int32` op, `Int32` op1, `Int32` op2, `Int32` lnb) | add an instruction | 
| `void` | AddJumpLabel(`String` label, `Int32` line) | add a jump label | 
| `Int32` | AddLibrary(`String` lib) | add a library metadata | 
| `Int32` | AddString(`String` s) | add a string constant  return the index of the added string constant | 
| `Program` | Emit() | emit the program | 
| `Int32` | GetJumpLabel(`String` s) | get a jump label's target | 
| `Int32` | GetLibrary(`String` lib) | get a library index | 
| `Int32` | GetString(`String` s) | retrieve a string constant  return -1 if string constant is not exist | 


