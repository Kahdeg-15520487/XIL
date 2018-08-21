using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using XIL.VM;

namespace testconsole
{
    /// <summary>
    /// run xse file
    /// </summary>
    class Run
    {
        //public static int run(string path = null) {
        //	Console.WriteLine("run {0}", path is null ? "null" : path);
        //	if (path is null) {
        //		Console.WriteLine("please enter path");
        //		return 1;
        //	}
        //	if (!File.Exists(path)) {
        //		Console.WriteLine("path does not exist");
        //		return 2;
        //	}
        //	var vm = new VirtualMachine(Program.Libs.ToArray());
        //	var bytecode = LoadBinary(path);
        //	vm.LoadProgram(bytecode);
        //	vm.Run();
        //	return 0;
        //}

        public static int run(string[] paths)
        {
            Console.WriteLine("run {0}", string.Join(" ", paths));
            if (paths.GetLength(0) == 0)
            {
                Console.WriteLine("please enter path");
                return 1;
            }
            foreach (var path in paths)
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine($"{path} does not exist");
                    return 2;
                }
            }
            var vm = new VirtualMachine(Program.Libs.ToArray());
            VirtualMachine.RandomNumberGenerator = new Random(0);
            foreach (var path in paths)
            {
                var (instrs, strs) = LoadBinary(path);
                vm.LoadProgram(instrs, strs);
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            vm.Run();
            stopwatch.Stop();
            Console.WriteLine($"Execution take {stopwatch.ElapsedMilliseconds} ms");
            return 0;
        }

        static (XIL.LangDef.Instruction[] instrs, string[] strs) LoadBinary(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException(filename);
            }

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                XIL.VM.Program.Deserialize(fs, out XIL.LangDef.Instruction[] instrs, out string[] strs);

                return (instrs, strs);
            }
        }
    }
}
