using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using XIL.Assembler;
using XIL.Assembler.Preprocessor;
using XIL.LangDef;

namespace testconsole
{
    /// <summary>
    /// Compile xil file to xse file
    /// </summary>
    class Compile
    {

        public static int compile(string path = null, string save = null)
        {
            Console.WriteLine("compiling {0}", path is null ? "null" : path);
            if (path is null)
            {
                Console.WriteLine("please enter path");
                return 1;
            }
            if (!File.Exists(path))
            {
                Console.WriteLine("path does not exist");
                return 2;
            }
            var compiler = new Assembler(Program.Libs.ToArray());
            var sourcecode = File.ReadAllText(path) + Environment.NewLine;

            Preprocessor preprocessor = new Preprocessor();
            sourcecode = preprocessor.Process(sourcecode);
            if (!preprocessor.IsSuccess)
            {
                Console.WriteLine("error with preprocessor");
                return 0;
            }

            var result = compiler.Compile(sourcecode);
            Console.WriteLine(result.Success ? "sucess" : "false" + result.Message);
            if (result.Success)
            {
                string savename;
                if (save is null)
                {
                    savename = Path.GetDirectoryName(Path.GetFullPath(path)) + '\\' + Path.GetFileNameWithoutExtension(path) + ".xse";
                }
                else
                {
                    savename = save;
                }
                Console.WriteLine(Path.GetFullPath(savename));
                var program = result.CodeGenerator.Emit();
                StoreBinary(savename, program);
            }
            return 0;
        }

        static void StoreBinary(string filename, XIL.VM.Program program)
        {
            if (File.Exists(filename))
            {
                //todo check file and if it exist, ask if want to overwrite
            }
            using (var fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                XIL.VM.Program.Serialize(fs, program);
            }
        }
    }
}
