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
| `Instruction` | Nop | nop | 


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
| `0` | jmp | jump lable  jump to the given label | 
| `1` | je | je lable  jump to the given label if a = b | 
| `2` | jne | jne lable  jump to the given label if a != b | 
| `3` | jg | jg lable  jump to the given label if a &gt; b | 
| `4` | jge | jge lable  jump to the given label if a &gt;= b | 
| `5` | jl | jl lable  jump to the given label if a &lt; b | 
| `6` | jle | jle lable  jump to the given label if a &lt;= b | 
| `7` | j1 | j1 lable  jump to the given label if tots is 1 | 
| `8` | j0 | j0 lable  jump to the given label if tots is 0 | 
| `9` | jel | jel lable literal  jump to the given label if a == literal | 
| `10` | jnel | jnel lable literal  jump to the given label if a != literal | 
| `11` | exit | exit  stop executing, set exit code to tots and clear the stack | 
| `12` | pause | pause  wait for the &lt;tots&gt; ms | 
| `13` | halt | halt  halt execution, can only be unhalt by another thread or by the vm | 
| `14` | unhalt | unhalt  unhalt another thread indicate by &lt;tots&gt; | 
| `15` | nop | nop  do nothing | 
| `16` | add | add  add 2 tots and push the result | 
| `17` | sub | sub  subtract 2 tots and push the result | 
| `18` | mul | mul  multiply 2 tots and push the result | 
| `19` | div | div  divide 2 tots and push the result | 
| `20` | mod | mod  modulus 2 tots and push the result | 
| `21` | dec | dec  decrement tots | 
| `22` | inc | inc  increment tots | 
| `23` | neg | neg  negate tots | 
| `24` | cmp | cmp  cmp 2 tots and push 1, 0 or -1 | 
| `25` | push | push &lt;var&gt;  push the value of &lt;var&gt; on tots | 
| `26` | yeet | yeet &lt;literal&gt;  yeet &lt;literal&gt; on tots | 
| `27` | pop | pop &lt;var&gt;  pop the value of tots into &lt;var&gt; | 
| `28` | load | load &lt;var&gt; &lt;literal&gt;  load &lt;literal&gt; into &lt;var&gt; | 
| `29` | getstack | gets  push the content of the value at the stack index  which is on tots onto the tots | 
| `30` | setstack | sets  set the content of the value at the stack index  which is on second tots to tots | 
| `31` | copy | copy &lt;var1&gt; &lt;var2&gt;  copy value of &lt;var2&gt; into &lt;var1&gt; | 
| `32` | dup | dup  duplicate tots | 
| `33` | swap | swap  swap 2 tots | 
| `34` | remove | remove  remove tots | 
| `35` | rand | rand  push a random value on tots | 
| `36` | randmax | rand  push a random value on tots | 
| `37` | call | call &lt;label&gt;  store the current ic into return target and jump to the given label | 
| `38` | ret | ret  jump to the return target | 
| `39` | brp | brp  break execution | 
| `40` | req | req &lt;library's name&gt;  check VM for certain library | 


