namespace XIL.VM
{
    /// <summary>
    /// instruction delegate
    /// </summary>
    public delegate void InstructionAction(Thread thread, int operand1, int operand2);
}