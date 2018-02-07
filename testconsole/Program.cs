﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using XIL.VM;
using XIL.Assembler;
using XIL.LangDef;
using System.Reflection;

namespace testconsole {
	class Program {
		public static List<IInstructionImplementation> Libs = null;
		public static BuiltinInstruction builtinInstruction = new BuiltinInstruction();
		public static IOInstruction ioInstruction = new IOInstruction();
		public static DiagnosticInstruction diagnosticInstruction = new DiagnosticInstruction();

		public static int run(CommandLineArguments arg) {
			return CommandLine.Run<Run>(arg, "run");
		}

		public static int compile(CommandLineArguments arg) {
			return CommandLine.Run<Compile>(arg, "compile");
		}

		public static int loadlibrary(string[] paths = null) {
			Console.WriteLine("load lib {0}", paths is null ? "null" : string.Join(", ", paths));
			return 0;
		}

		public static int help() {
			Console.WriteLine("halp");
			return 0;
		}

		static int Main(string[] args) {
			//test load from external assemblies IInstructionImplementation


			return 0;

			//test commandline 
			if (Libs is null) {
				Libs = new List<IInstructionImplementation> {
					builtinInstruction,
					ioInstruction
				};
			}
			var exitCode = CommandLine.Run<Program>(new CommandLineArguments(args), defaultCommandName: "help");
			Console.ReadLine();
			return exitCode;

			//test vm function
			VirtualMachine vm = new VirtualMachine(builtinInstruction, ioInstruction, diagnosticInstruction);

			//var fibonacy = new List<Instruction>(){
			//	new Instruction(InstructionOPCode.yeet,0),
			//	new Instruction(InstructionOPCode.yeet,1),
			//	new Instruction(InstructionOPCode.push,0),
			//	new Instruction(InstructionOPCode.push,1),
			//	new Instruction(InstructionOPCode.add),
			//	new Instruction(InstructionOPCode.dup),
			//	new Instruction(InstructionOPCode.dup),
			//	new Instruction(0x30),
			//	new Instruction(InstructionOPCode.copy,0,1),
			//	new Instruction(InstructionOPCode.pop,1),
			//	new Instruction(InstructionOPCode.yeet,200),
			//	new Instruction(InstructionOPCode.jg,2)
			//};

			//var dice = new List<Instruction>() {
			//	new Instruction(InstructionOPCode.yeet,0),
			//	new Instruction(InstructionOPCode.randmax,6),
			//	new Instruction(InstructionOPCode.pop,0),
			//	new Instruction(InstructionOPCode.push,0),
			//	new Instruction(0x30)
			//};

			//var intencode = new List<Instruction>() {
			//	new Instruction(InstructionOPCode.yeet,0),
			//	new Instruction(0x30),
			//	new Instruction(InstructionOPCode.pop,0),
			//	new Instruction(InstructionOPCode.push,0),
			//	new Instruction(InstructionOPCode.randmax,2)
			//};

			//int[] bytecode = null;
			//if (args.Length == 1) {
			//	try {
			//		bytecode = LoadBinary(args[0]);
			//	}
			//	catch (Exception e) {
			//		Console.WriteLine(e.Message);
			//	}
			//}

			//List<Instruction> programtorun;

			//if (bytecode != null) {
			//	programtorun = VirtualMachine.Deserialize(bytecode).ToList();
			//}
			//else {
			//	programtorun = intencode;
			//}

			//if (vm.LoadProgram(programtorun)) {
			//	vm.Run();
			//}
			//else {
			//	Console.WriteLine("Can't load program");
			//}

			Assembler assembler = new Assembler(builtinInstruction, ioInstruction, diagnosticInstruction);
			string code = @"
start:
yeet 1
print
jump start
";
			var result = assembler.Compile(code);
			Console.WriteLine(result.Message);
			if (result.CompiledProgram != null) {
				if (vm.LoadProgram(result.CompiledProgram)) {
					vm.Run();
				}
				else {
					Console.WriteLine("Can't load program");
				}
			}
			Console.ReadLine();
		}

		public static List<T> GetFilePlugins<T>(string filename) {
			List<T> ret = new List<T>();
			if (File.Exists(filename)) {
				Type typeT = typeof(T);

				Assembly assembly = Assembly.LoadFrom(filename);
				foreach (Type type in assembly.GetTypes()) {
					if (!type.IsClass || type.IsNotPublic) continue;
					if (typeT.IsAssignableFrom(type)) {
						T plugin = (T)Activator.CreateInstance(type);
						ret.Add(plugin);
					}
				}
			}
			return ret;
		}
	}
}
