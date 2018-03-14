using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XIL.VM;

namespace testconsole {
	/// <summary>
	/// run xse file
	/// </summary>
	class Run {
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

		public static int run(string[] paths) {
			Console.WriteLine("run {0}", string.Join(" ", paths));
			if (paths.GetLength(0) == 0) {
				Console.WriteLine("please enter path");
				return 1;
			}
			foreach (var path in paths) {
				if (!File.Exists(path)) {
					Console.WriteLine($"{path} does not exist");
					return 2;
				}
			}
			var vm = new VirtualMachine(Program.Libs.ToArray());
			foreach (var path in paths) {
				var bytecode = LoadBinary(path);
				vm.LoadProgram(bytecode);
			}
			vm.Run();
			return 0;
		}

		static int[] LoadBinary(string filename) {
			if (!File.Exists(filename)) {
				throw new FileNotFoundException(filename);
			}
			byte[] binary = null;
			try {
				binary = File.ReadAllBytes(filename);
				foreach (var b in binary) {

				}
			}
			catch (Exception e) {

				throw e;
			}

			if (binary.Length % 3 != 0) {
				throw new InvalidDataException("the byte stream is malformed");
			}

			int length = binary.Length / 4;
			int[] result = new int[length];
			using (BinaryReader br = new BinaryReader(new MemoryStream(binary))) {
				for (int i = 0; i < length; i++) {
					result[i] = br.ReadInt32();
				}
			}
			return result;
		}
	}
}
