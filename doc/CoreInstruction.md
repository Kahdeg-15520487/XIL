## `CoreInstruction`

```csharp
public class CoreInstruction
    : IInstructionImplementation

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Add(`Thread` thread, `Int32` operand1, `Int32` operand2) | add ,  add 2 tots and push the result | 
| `void` | BreakPoint(`Thread` thread, `Int32` operand1, `Int32` operand2) | breakpoint ,  set breakpoint | 
| `void` | Call(`Thread` thread, `Int32` operand1, `Int32` operand2) | call &lt;label&gt; ,  store the current ic into return target and jump to the given label | 
| `void` | Compare(`Thread` thread, `Int32` operand1, `Int32` operand2) | cmp ,  cmp 2 tots and push 1, 0 or -1 | 
| `void` | Copy(`Thread` thread, `Int32` operand1, `Int32` operand2) | copy &lt;var1&gt; &lt;var2&gt; ,  copy value of &lt;var2&gt; into &lt;var1&gt; | 
| `void` | Decrement(`Thread` thread, `Int32` operand1, `Int32` operand2) | dec ,  decrement tots | 
| `void` | Divide(`Thread` thread, `Int32` operand1, `Int32` operand2) | div ,  divide 2 tots and push the result | 
| `void` | Duplicate(`Thread` thread, `Int32` operand1, `Int32` operand2) | dup ,  duplicate tots | 
| `void` | Exit(`Thread` thread, `Int32` operand1, `Int32` operand2) | exit ,  stop executing, set exit code to tots and clear the stack | 
| `void` | GetStack(`Thread` thread, `Int32` operand1, `Int32` operand2) | gets ,  push the content of the value at the stack index ,  which is on tots onto the tots | 
| `void` | Halt(`Thread` thread, `Int32` operand1, `Int32` operand2) | halt ,  halt execution, can only be unhalt by another thread or by the vm | 
| `void` | Increment(`Thread` thread, `Int32` operand1, `Int32` operand2) | inc ,  increment tots | 
| `void` | Jump(`Thread` thread, `Int32` operand1, `Int32` operand2) | jump lable ,  jump to the given label | 
| `void` | JumpEqual(`Thread` thread, `Int32` operand1, `Int32` operand2) | je lable ,  jump to the given label if a = b | 
| `void` | JumpEqualLiteral(`Thread` thread, `Int32` operand1, `Int32` operand2) | jel lable literal ,  jump to the given label if a == literal | 
| `void` | JumpFalse(`Thread` thread, `Int32` operand1, `Int32` operand2) | j0 lable ,  jump to the given label if a == 0 | 
| `void` | JumpGreater(`Thread` thread, `Int32` operand1, `Int32` operand2) | jg lable ,  jump to the given label if a &gt; b | 
| `void` | JumpGreaterOrEqual(`Thread` thread, `Int32` operand1, `Int32` operand2) | jge lable ,  jump to the given label if a &gt;= b | 
| `void` | JumpLesser(`Thread` thread, `Int32` operand1, `Int32` operand2) | jl lable ,  jump to the given label if a &lt; b | 
| `void` | JumpLesserOrEqual(`Thread` thread, `Int32` operand1, `Int32` operand2) | jle lable ,  jump to the given label if a &lt;= b | 
| `void` | JumpNotEqual(`Thread` thread, `Int32` operand1, `Int32` operand2) | jne lable ,  jump to the given label if a != b | 
| `void` | JumpNotEqualLiteral(`Thread` thread, `Int32` operand1, `Int32` operand2) | jnel lable literal ,  jump to the given label if a != literal | 
| `void` | JumpTrue(`Thread` thread, `Int32` operand1, `Int32` operand2) | j1 lable ,  jump to the given label if a == 1 | 
| `void` | Load(`Thread` thread, `Int32` operand1, `Int32` operand2) | load &lt;var&gt; &lt;literal&gt; ,  load &lt;literal&gt; into &lt;var&gt; | 
| `void` | Modulus(`Thread` thread, `Int32` operand1, `Int32` operand2) | mod ,  modulus 2 tots and push the result | 
| `void` | Multiply(`Thread` thread, `Int32` operand1, `Int32` operand2) | mul ,  multiply 2 tots and push the result | 
| `void` | Negate(`Thread` thread, `Int32` operand1, `Int32` operand2) | neg ,  negate tots | 
| `void` | Nooperation(`Thread` thread, `Int32` operand1, `Int32` operand2) | nop ,  do nothing | 
| `void` | Pause(`Thread` thread, `Int32` operand1, `Int32` operand2) | pause ,  wait for the &lt;tots&gt; ms | 
| `void` | Pop(`Thread` thread, `Int32` operand1, `Int32` operand2) | pop &lt;var&gt; ,  pop the value of tots into &lt;var&gt; | 
| `void` | Push(`Thread` thread, `Int32` operand1, `Int32` operand2) | push &lt;var&gt; ,  push the value of &lt;var&gt; on tots | 
| `void` | Random(`Thread` thread, `Int32` operand1, `Int32` operand2) | rand ,  push a random value on tots | 
| `void` | RandomMax(`Thread` thread, `Int32` operand1, `Int32` operand2) | randmax &lt;max&gt; ,  push a random value on tots | 
| `void` | Remove(`Thread` thread, `Int32` operand1, `Int32` operand2) | remove ,  remove tots | 
| `void` | Return(`Thread` thread, `Int32` operand1, `Int32` operand2) | ret ,  jump to the return target | 
| `void` | SetStack(`Thread` thread, `Int32` operand1, `Int32` operand2) | sets ,  set the content of the value at the stack index ,  which is on second tots to tots | 
| `void` | Substract(`Thread` thread, `Int32` operand1, `Int32` operand2) | sub ,  subtract 2 tots and push the result | 
| `void` | Swap(`Thread` thread, `Int32` operand1, `Int32` operand2) | swap ,  swap 2 tots | 
| `void` | Unhalt(`Thread` thread, `Int32` operand1, `Int32` operand2) | unhalt ,  unhalt another thread indicate by &lt;tots&gt; | 
| `void` | Yeet(`Thread` thread, `Int32` operand1, `Int32` operand2) | yeet &lt;literal&gt; ,  yeet &lt;literal&gt; on tots | 


