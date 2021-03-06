﻿using System;
using System.Collections.Generic;
using System.Linq;
using XIL.LangDef;
using XIL.VM;

/// <summary>
/// core instruction
/// </summary>
public sealed class CoreInstruction : IInstructionImplementation
{
    #region flow control
    /// <summary>
    /// jump lable <para />
    /// jump to the given label
    /// </summary>
    [Instruction(InstructionOPCode.jmp, "j")]
    public void Jump(Thread thread, int operand1, int operand2)
    {
        thread.currentInstruction = operand1;
    }
    /// <summary>
    /// je lable <para />
    /// jump to the given label if a = b
    /// </summary>
    [Instruction(InstructionOPCode.je, "je")]
    public void JumpEqual(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        int op2 = thread.Pop();
        if (op1 == op2)
        {
            thread.currentInstruction = operand1;
        }
    }
    /// <summary>
    /// jne lable <para />
    /// jump to the given label if a != b
    /// </summary>
    [Instruction(InstructionOPCode.jne, "jne")]
    public void JumpNotEqual(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        int op2 = thread.Pop();
        if (op1 != op2)
        {
            thread.currentInstruction = operand1;
        }
    }
    /// <summary>
    /// jg lable <para />
    /// jump to the given label if a > b
    /// </summary>
    [Instruction(InstructionOPCode.jg, "jg")]
    public void JumpGreater(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        int op2 = thread.Pop();
        if (op1 > op2)
        {
            thread.currentInstruction = operand1;
        }
    }
    /// <summary>
    /// jge lable <para />
    /// jump to the given label if a >= b
    /// </summary>
    [Instruction(InstructionOPCode.jge, "jge")]
    public void JumpGreaterOrEqual(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        int op2 = thread.Pop();
        if (op1 >= op2)
        {
            thread.currentInstruction = operand1;
        }
    }
    /// <summary>
    /// jl lable <para />
    /// jump to the given label if a &lt; b
    /// </summary>
    [Instruction(InstructionOPCode.jl, "jl")]
    public void JumpLesser(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        int op2 = thread.Pop();
        if (op1 < op2)
        {
            thread.currentInstruction = operand1;
        }
    }
    /// <summary>
    /// jle lable <para />
    /// jump to the given label if a &lt;= b
    /// </summary>
    [Instruction(InstructionOPCode.jle, "jle")]
    public void JumpLesserOrEqual(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        int op2 = thread.Pop();
        if (op1 <= op2)
        {
            thread.currentInstruction = operand1;
        }
    }
    /// <summary>
    /// j1 lable <para />
    /// jump to the given label if a == 1
    /// </summary>
    [Instruction(InstructionOPCode.j1, "j1")]
    public void JumpTrue(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        if (op1 == 1)
        {
            thread.currentInstruction = operand1;
        }
    }
    /// <summary>
    /// j0 lable <para />
    /// jump to the given label if a == 0
    /// </summary>
    [Instruction(InstructionOPCode.j0, "j0")]
    public void JumpFalse(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        if (op1 == 0)
        {
            thread.currentInstruction = operand1;
        }
    }
    /// <summary>
    /// jel lable literal <para />
    /// jump to the given label if a == literal
    /// </summary>
    [Instruction(InstructionOPCode.jel, "jel")]
    public void JumpEqualLiteral(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        int op2 = operand2;
        if (op1 == op2)
        {
            thread.currentInstruction = operand1;
        }
    }
    /// <summary>
    /// jnel lable literal <para />
    /// jump to the given label if a != literal
    /// </summary>
    [Instruction(InstructionOPCode.jnel, "jnel")]
    public void JumpNotEqualLiteral(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        int op2 = operand2;
        if (op1 != op2)
        {
            thread.currentInstruction = operand1;
        }
    }
    #endregion

    #region arthimetic
    /// <summary>
    /// add <para />
    /// add 2 tots and push the result
    /// </summary>
    [Instruction(InstructionOPCode.add, "add")]
    public void Add(Thread thread, int operand1, int operand2)
    {
        int op2 = thread.Pop();
        int op1 = thread.Pop();
        int result = op1 + op2;
        thread.Push(result);
    }
    /// <summary>
    /// sub <para />
    /// subtract 2 tots and push the result
    /// </summary>
    [Instruction(InstructionOPCode.sub, "sub")]
    public void Substract(Thread thread, int operand1, int operand2)
    {
        int op2 = thread.Pop();
        int op1 = thread.Pop();
        int result = op1 - op2;
        thread.Push(result);
    }
    /// <summary>
    /// mul <para />
    /// multiply 2 tots and push the result
    /// </summary>
    [Instruction(InstructionOPCode.mul, "mul")]
    public void Multiply(Thread thread, int operand1, int operand2)
    {
        int op2 = thread.Pop();
        int op1 = thread.Pop();
        int result = op1 * op2;
        thread.Push(result);
    }
    /// <summary>
    /// div <para />
    /// divide 2 tots and push the result
    /// </summary>
    [Instruction(InstructionOPCode.div, "div")]
    public void Divide(Thread thread, int operand1, int operand2)
    {
        int op2 = thread.Pop();
        int op1 = thread.Pop();
        int result = op1 / op2;
        thread.Push(result);
    }
    /// <summary>
    /// mod <para />
    /// modulus 2 tots and push the result
    /// </summary>
    [Instruction(InstructionOPCode.mod, "mod")]
    public void Modulus(Thread thread, int operand1, int operand2)
    {
        int op2 = thread.Pop();
        int op1 = thread.Pop();
        int result = op1 % op2;
        thread.Push(result);
    }


    /// <summary>
    /// dec <para />
    /// decrement tots
    /// </summary>
    [Instruction(InstructionOPCode.dec, "dec")]
    public void Decrement(Thread thread, int operand1, int operand2)
    {
        int op = thread.Pop();
        int result = op - 1;
        thread.Push(result);
    }
    /// <summary>
    /// inc <para />
    /// increment tots
    /// </summary>
    [Instruction(InstructionOPCode.inc, "inc")]
    public void Increment(Thread thread, int operand1, int operand2)
    {
        int op = thread.Pop();
        int result = op + 1;
        thread.Push(result);
    }

    /// <summary>
    /// neg <para />
    /// negate tots
    /// </summary>
    [Instruction(InstructionOPCode.neg, "neg")]
    public void Negate(Thread thread, int operand1, int operand2)
    {
        int op = thread.Pop();
        int result = -op;
        thread.Push(result);
    }

    /// <summary>
    /// cmp <para />
    /// cmp 2 tots and push 1, 0 or -1
    /// </summary>
    [Instruction(InstructionOPCode.cmp, "cmp")]
    public void Compare(Thread thread, int operand1, int operand2)
    {
        int op2 = thread.Pop();
        int op1 = thread.Pop();
        int result = op1 > op2 ? 1 : op1 < op2 ? -1 : 0;
        thread.Push(result);
    }

    #endregion

    #region stack manipulation
    /// <summary>
    /// push &lt;var&gt; <para />
    /// push the value of &lt;var&gt; on tots
    /// </summary>
    [Instruction(InstructionOPCode.push, "push")]
    public void Push(Thread thread, int operand1, int operand2)
    {
        int value = thread.Get(operand1);
        thread.Push(value);
    }

    /// <summary>
    /// yeet &lt;literal&gt; <para />
    /// yeet &lt;literal&gt; on tots
    /// </summary>
    [Instruction(InstructionOPCode.yeet, "yeet")]
    public void Yeet(Thread thread, int operand1, int operand2)
    {
        thread.Push(operand1);
    }

    /// <summary>
    /// pop &lt;var&gt; <para />
    /// pop the value of tots into &lt;var&gt;
    /// </summary>
    [Instruction(InstructionOPCode.pop, "pop")]
    public void Pop(Thread thread, int operand1, int operand2)
    {
        int value = thread.Pop();
        thread.Set(operand1, value);
    }

    /// <summary>
    /// load &lt;var&gt; &lt;literal&gt; <para />
    /// load &lt;literal&gt; into &lt;var&gt;
    /// </summary>
    [Instruction(InstructionOPCode.load, "load")]
    public void Load(Thread thread, int operand1, int operand2)
    {
        thread.Set(operand1, operand2);
    }

    /// <summary>
    /// gets <para />
    /// push the content of the value at the stack index <para/>
    /// which is on tots onto the tots
    /// </summary>
    [Instruction(InstructionOPCode.getstack, "gets")]
    public void GetStack(Thread thread, int operand1, int operand2)
    {
        int stackIndex = thread.Pop();
        int value = thread.Get(stackIndex);
        thread.Push(value);
    }

    /// <summary>
    /// sets <para />
    /// set the content of the value at the stack index <para/>
    /// which is on second tots to tots
    /// </summary>
    [Instruction(InstructionOPCode.setstack, "sets")]
    public void SetStack(Thread thread, int operand1, int operand2)
    {
        int value = thread.Pop();
        int stackIndex = thread.Pop();
        thread.Set(stackIndex, value);
    }


    /// <summary>
    /// copy &lt;var1&gt; &lt;var2&gt; <para />
    /// copy value of &lt;var2&gt; into &lt;var1&gt;
    /// </summary>
    [Instruction(InstructionOPCode.copy, "copy")]
    public void Copy(Thread thread, int operand1, int operand2)
    {
        int var2 = thread.Get(operand2);
        thread.Set(operand1, var2);
    }

    /// <summary>
    /// dup <para/>
    /// duplicate tots
    /// </summary>
    [Instruction(InstructionOPCode.dup, "dup")]
    public void Duplicate(Thread thread, int operand1, int operand2)
    {
        int op = thread.Peek();
        thread.Push(op);
    }

    /// <summary>
    /// swap <para/>
    /// swap 2 tots
    /// </summary>
    [Instruction(InstructionOPCode.swap, "swap")]
    public void Swap(Thread thread, int operand1, int operand2)
    {
        int op1 = thread.Pop();
        int op2 = thread.Pop();
        thread.Push(op1);
        thread.Push(op2);
    }

    /// <summary>
    /// remove <para/>
    /// remove tots
    /// </summary>
    [Instruction(InstructionOPCode.remove, "rm")]
    public void Remove(Thread thread, int operand1, int operand2)
    {
        thread.Pop();
    }

    #endregion

    #region execution manipulation
    /// <summary>
    /// exit <para/>
    /// stop executing, set exit code to tots and clear the stack 
    /// </summary>
    [Instruction(InstructionOPCode.exit, "exit")]
    public void Exit(Thread thread, int operand1, int operand2)
    {
        int op = thread.IsStackEmpty ? 0 : thread.Pop();
        thread.Clear();
        thread.ExitCode = op;
        thread.EndExecution();
    }

    /// <summary>
    /// pause <para/>
    /// wait for the &lt;tots&gt; ms
    /// </summary>
    [Instruction(InstructionOPCode.pause, "pause")]
    public void Pause(Thread thread, int operand1, int operand2)
    {

    }

    /// <summary>
    /// halt <para/>
    /// halt execution, can only be unhalt by another thread or by the vm
    /// </summary>
    [Instruction(InstructionOPCode.halt, "halt")]
    public void Halt(Thread thread, int operand1, int operand2)
    {

    }

    /// <summary>
    /// unhalt <para/>
    /// unhalt another thread indicate by &lt;tots&gt;
    /// </summary>
    [Instruction(InstructionOPCode.unhalt, "unhalt")]
    public void Unhalt(Thread thread, int operand1, int operand2)
    {

    }

    #endregion

    #region function calling
    /// <summary>
    /// call &lt;label&gt; <para />
    /// store the current ic into return target and jump to the given label
    /// </summary>
    [Instruction(InstructionOPCode.call, "call")]
    public void Call(Thread thread, int operand1, int operand2)
    {
        thread.PushF(thread.currentInstruction);
        thread.currentInstruction = operand1;
    }

    /// <summary>
    /// ret <para />
    /// jump to the return target
    /// </summary>
    [Instruction(InstructionOPCode.ret, "ret")]
    public void Return(Thread thread, int operand1, int operand2)
    {
        if (thread.CurrentFStack == 0)
        {
            thread.EndExecution();
        }
        else
        {
            thread.currentInstruction = thread.PopF();
        }
    }

    #endregion

    #region miscellaneous
    /// <summary>
    /// rand <para />
    /// push a random value on tots
    /// </summary>
    [Instruction(InstructionOPCode.rand, "rand")]
    public void Random(Thread thread, int operand1, int operand2)
    {
        int result = VirtualMachine.RandomNumberGenerator.Next();
        thread.Push(result);
    }

    /// <summary>
    /// randseed &lt;var&gt; <para />
    /// set the random seed of the vm
    /// </summary>
    [Instruction(InstructionOPCode.randseed, "randseed")]
    public void RandSeed(Thread thread, int operand1, int operand2)
    {
        VirtualMachine.RandomNumberGenerator = new Random(operand1);
    }

    /// <summary>
    /// randmax &lt;max&gt; <para />
    /// push a random value on tots
    /// </summary>
    [Instruction(InstructionOPCode.randmax, "randmax")]
    public void RandomMax(Thread thread, int operand1, int operand2)
    {
        int result = VirtualMachine.RandomNumberGenerator.Next(operand1);
        thread.Push(result);
    }

    /// <summary>
    /// randmax &lt;max&gt; <para />
    /// push a random value on tots
    /// </summary>
    [Instruction(InstructionOPCode.randrange, "randrange")]
    public void RandomInRange(Thread thread, int operand1, int operand2)
    {
        int result = VirtualMachine.RandomNumberGenerator.Next(operand1, operand2);
        thread.Push(result);
    }

    /// <summary>
    /// nop <para />
    /// do nothing
    /// </summary>
    [Instruction(InstructionOPCode.nop, "nop")]
    public void Nooperation(Thread thread, int operand1, int operand2)
    {

    }

    /// <summary>
    /// breakpoint <para />
    /// set breakpoint
    /// </summary>
    [Instruction(InstructionOPCode.brp, "brp")]
    public void BreakPoint(Thread thread, int operand1, int operand2)
    {
        //todo pause execution and print stack frame
        //print stack frame
        Console.WriteLine("== Top of the stack  ==");
        int[] stack = thread.stack.SnapShot().Reverse().ToArray();
        for (int i = 0; i < stack.Length; i++)
        {
            Console.WriteLine("{1} {0}", stack[i], (stack.Length - 1 - i).ToString().PadRight(5));
        }
        Console.WriteLine("==Bottom of the stack==");
        Console.WriteLine("Press enter to continue...");
        Console.ReadLine();
    }
    #endregion
}