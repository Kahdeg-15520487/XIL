using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XIL.LangDef;
using XIL.VM;

namespace testconsole
{
    public class FileInstruction : IInstructionImplementation
    {
        static string filePath = null;
        static string fileContent = null;
        static int cursorPos = 0;

        /// <summary>
		/// open &lt;path&gt; <para/>
		/// open the file at path to read and write
		/// </summary>
		[Instruction(0x60, "openf")]
        public void OpenFile(Thread thread, int operand1, int operand2)
        {
            //todo implement file open
            //open a file if it exist, load the file content into fileContent
            var path = thread.GetString(operand1);
            if (!File.Exists(path))
            {
                Console.WriteLine("{0} not found", path);
                Console.WriteLine("execution terminated");
                thread.EndExecution();
                return;
            }

            filePath = path;
            fileContent = File.ReadAllText(filePath);
            cursorPos = 0;
        }

        /// <summary>
		/// creatf &lt;path&gt; <para/>
		/// create a file at path
		/// </summary>
		[Instruction(0x61, "createf")]
        public void CreateFile(Thread thread, int operand1, int operand2)
        {
            var path = thread.GetString(operand1);
            if (File.Exists(path))
            {
                Console.WriteLine("{0} already exist", path);
                Console.WriteLine("execution terminated");
                thread.EndExecution();
                return;
            }
            File.WriteAllText(path, "");
        }

        /// <summary>
		/// writec <para/>
		/// append the char at tots to the openning file
		/// </summary>
		[Instruction(0x62, "writec")]
        public void WriteChar(Thread thread, int operand1, int operand2)
        {
            if (fileContent is null)
            {
                Console.WriteLine("no file is currently openned");
                Console.WriteLine("execution terminated");
                thread.EndExecution();
                return;
            }
            //append the char to the end of fileContent
            char c = (char)thread.Pop();
            fileContent += c;
        }

        /// <summary>
		/// readc <para/>
		/// read the char at cursor pos and advance the cursor
		/// </summary>
		[Instruction(0x63, "readc")]
        public void ReadChar(Thread thread, int operand1, int operand2)
        {
            if (fileContent is null)
            {
                Console.WriteLine("no file is currently openned");
                Console.WriteLine("execution terminated");
                thread.EndExecution();
                return;
            }
            //read the char from the beginning of the fileContent and advance the cursor
            char c = fileContent[cursorPos++];
            thread.Push(c);
        }

        /// <summary>
		/// setcur <para/>
		/// set the cursor position to tots
		/// </summary>
		[Instruction(0x64, "setcur")]
        public void SetCursorPos(Thread thread, int operand1, int operand2)
        {
            //set the cursorPos to tots
            cursorPos = thread.Pop();
        }

        /// <summary>
		/// clearf <para/>
		/// clear the file content
		/// </summary>
		[Instruction(0x65, "clearf")]
        public void ClearFile(Thread thread, int operand1, int operand2)
        {
            if (fileContent is null)
            {
                Console.WriteLine("no file is currently openned");
                Console.WriteLine("execution terminated");
                thread.EndExecution();
                return;
            }
            //set fileContent to empty
            fileContent = string.Empty;
        }

        /// <summary>
		/// closef <para/>
		/// close the file
		/// </summary>
		[Instruction(0x66, "closef")]
        public void CloseFile(Thread thread, int operand1, int operand2)
        {
            if (fileContent is null)
            {
                Console.WriteLine("no file is currently openned");
                Console.WriteLine("execution terminated");
                thread.EndExecution();
                return;
            }
            //overwrite the file at filePath with fileContent
            File.WriteAllText(filePath, fileContent);

            filePath = null;
            fileContent = null;
        }
    }
}
