using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XIL.Assembler;
using XIL.Assembler.Preprocessor;
using XILtoCIL;

namespace testconsole
{
    class CILCompile
    {
        public static int compile(string path = null, string save = null, bool gendll = false) {
            Console.WriteLine("compiling {0}", path is null ? "null" : path);
            if (path is null) {
                Console.WriteLine("please enter path");
                return 1;
            }
            if (!File.Exists(path)) {
                Console.WriteLine("path does not exist");
                return 2;
            }
            Assembler compiler = new Assembler(Program.Libs.ToArray());
            string sourcecode = File.ReadAllText(path) + Environment.NewLine;

            Preprocessor preprocessor = new Preprocessor();
            sourcecode = preprocessor.Process(sourcecode);
            if (!preprocessor.IsSuccess) {
                Console.WriteLine("error with preprocessor");
                return 3;
            }

            CilCodeGenerator codegen = new CilCodeGenerator();

            CompileResult result = compiler.Compile(sourcecode, codegen);
            Console.WriteLine(result.Success ? "sucess" : "false" + result.Message);
            if (result.Success) {
                string savename;
                if (save is null) {
                    savename = Path.GetDirectoryName(Path.GetFullPath(path)) + '\\' + Path.GetFileNameWithoutExtension(path) + ".xse";
                } else {
                    savename = save;
                }
                Console.WriteLine(Path.GetFullPath(savename));
                Func<int> program = codegen.EmitDynamicMethod();
                Console.WriteLine(program?.Invoke());
                if (gendll) {
                    codegen.EmitAssembly();
                }
            }
            return 0;
        }
    }
}
