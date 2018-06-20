## `IInstructionImplementation`

```csharp
public interface XIL.LangDef.IInstructionImplementation

```

## `Instruction`

```csharp
public struct XIL.LangDef.Instruction

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | firstOperand |  | 
| `Int32` | lineNumber |  | 
| `Int32` | opCode |  | 
| `Int32` | secondOperand |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ToString() |  | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | InstructionByteLength |  | 


Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Instruction` | Exit |  | 
| `Instruction` | Nop |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `IEnumerable<Instruction>` | Deserialize(`Int32[]` program) |  | 
| `Int32[]` | Serialize(`List<Instruction>` program) |  | 


## `InstructionAttribute`

```csharp
public class XIL.LangDef.InstructionAttribute
    : Attribute, _Attribute

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | OpCode |  | 
| `String` | OpName |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ToString() |  | 


## `InstructionOPCode`

```csharp
public enum XIL.LangDef.InstructionOPCode
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | jmp |  | 
| `1` | je |  | 
| `2` | jne |  | 
| `3` | jg |  | 
| `4` | jge |  | 
| `5` | jl |  | 
| `6` | jle |  | 
| `7` | j1 |  | 
| `8` | j0 |  | 
| `9` | jel |  | 
| `10` | jnel |  | 
| `11` | exit |  | 
| `12` | pause |  | 
| `13` | halt |  | 
| `14` | unhalt |  | 
| `15` | nop |  | 
| `16` | add |  | 
| `17` | sub |  | 
| `18` | mul |  | 
| `19` | div |  | 
| `20` | mod |  | 
| `21` | dec |  | 
| `22` | inc |  | 
| `23` | neg |  | 
| `24` | cmp |  | 
| `25` | push |  | 
| `26` | yeet |  | 
| `27` | pop |  | 
| `28` | load |  | 
| `29` | getstack |  | 
| `30` | setstack |  | 
| `31` | copy |  | 
| `32` | dup |  | 
| `33` | swap |  | 
| `34` | remove |  | 
| `35` | rand |  | 
| `36` | randmax |  | 
| `37` | call |  | 
| `38` | ret |  | 
| `39` | brp |  | 
| `40` | req |  | 


