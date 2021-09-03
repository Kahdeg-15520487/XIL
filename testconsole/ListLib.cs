using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XIL.VM;

namespace testconsole
{
    class ListLib
    {
        public static int listlib()
        {
            var vm = new VirtualMachine(VirtualMachineVerboseLevel.None, Program.Libs.ToArray());
            foreach (var lib in VirtualMachine.InstructionMetaDataMap.Values.GroupBy(i => i.Library).OrderBy(l => l.OrderBy(i => i.OpCode).First().OpCode))
            {
                Console.WriteLine("library: {0}", lib.Key);
                foreach (var instr in lib.OrderBy(i => i.OpCode))
                {
                    Console.WriteLine(instr);
                }
            }
            return 0;
        }
    }
}
