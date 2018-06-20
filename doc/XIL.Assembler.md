## `Assembler`

```csharp
public class XIL.Assembler.Assembler

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `CompileResult` | Compile(`String` sourcecode) |  | 


## `CodeGenerator`

```csharp
public class XIL.Assembler.CodeGenerator

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Instruction>` | program |  | 
| `List<String>` | stringTable |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AddInstruction(`Instruction` instruction) |  | 
| `void` | AddInstruction(`Int32` opcode, `Int32` op1 = 0, `Int32` op2 = 0, `Int32` lnb = 0) |  | 
| `void` | AddJumpLabel(`String` label, `Int32` linecount) |  | 
| `Int32` | AddString(`String` str) |  | 
| `Int32` | GetJumpTarget(`String` label) |  | 
| `Int32` | GetString(`String` str) |  | 
| `Program` | Serialize() |  | 


## `Lexer`

```csharp
public class XIL.Assembler.Lexer

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Char` | current_char |  | 
| `Int32` | current_line |  | 
| `Int32` | pos |  | 
| `String` | source_code |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `List<Token>` | GetAllToken() |  | 
| `Token` | GetNextToken() |  | 
| `Token` | PeekNextToken() |  | 
| `void` | Reset() |  | 


## `Parser`

```csharp
public class XIL.Assembler.Parser

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `CodeGenerator` | CodeGen |  | 
| `Token` | CurrentToken |  | 
| `VariableScope` | GlobalScope |  | 
| `Int32` | InstructionCounter |  | 
| `Lexer` | Lexer |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Parse() |  | 
| `void` | ParseAllLabel() |  | 


## `Token`

```csharp
public class XIL.Assembler.Token

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | lexeme |  | 
| `TokenType` | tokenType |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | GetHashCode() |  | 
| `String` | ToString() |  | 


## `TokenType`

```csharp
public enum XIL.Assembler.TokenType
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | INT |  | 
| `1` | STRING |  | 
| `2` | BOOL |  | 
| `3` | VAR |  | 
| `4` | LABEL |  | 
| `5` | IDENT |  | 
| `6` | NEWLINE |  | 
| `7` | INVALID |  | 
| `8` | EOF |  | 
| `9` | ANY |  | 


## `VariableScope`

```csharp
public class XIL.Assembler.VariableScope

```

Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `VariableScope` | childScope |  | 
| `Dictionary<String, Int32>` | vars |  | 


Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | CurrentStackslot |  | 
| `Int32` | Item |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AddChildScope(`VariableScope` child) |  | 
| `void` | AddSymbol(`String` symbol) |  | 


