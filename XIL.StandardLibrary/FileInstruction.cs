using System;
using System.IO;

using XIL.LangDef;
using XIL.VM;

namespace XIL.StandardLibrary
{
    public class FileInstruction : IInstructionImplementation
    {
        const string lib = "file";

        static string filePath = null;
        static string fileContent = null;
        static int cursorPos = 0;

        /// <summary>
		/// open &lt;path&gt; <para/>
		/// open the file at path to read and write
		/// </summary>
		[Instruction(0x60, "openf", lib)]
        public void OpenFile(Thread thread, int operand1, int operand2)
        {
            //todo implement file open
            //open a file if it exist, load the file content into fileContent
            var path = thread.GetString(operand1);
            if (!File.Exists(path))
            {
                thread.RuntimeError(string.Format("{0} not found", path));
                return;
            }

            filePath = path;
            fileContent = File.ReadAllText(filePath);
            cursorPos = 0;
        }

        /// <summary>
        /// exist &lt;path&gt; <para/>
        /// check if there is a file exist at path
        /// </summary>
        [Instruction(0x61, "existf", lib)]
        public void ExistFile(Thread thread, int operand1, int operand2)
        {
            var path = thread.GetString(operand1);
            var value = File.Exists(path) ? 1 : 0;
            thread.Push(value);
        }

        /// <summary>
		/// creatf &lt;path&gt; <para/>
		/// create a file at path
		/// </summary>
		[Instruction(0x62, "createf", lib)]
        public void CreateFile(Thread thread, int operand1, int operand2)
        {
            var path = thread.GetString(operand1);
            if (File.Exists(path))
            {
                thread.RuntimeError(string.Format("{0} already exist", path));
                return;
            }
            File.WriteAllText(path, "");
        }

        /// <summary>
		/// writef <para/>
		/// append the char at tots to the openning file
		/// </summary>
		[Instruction(0x63, "writef", lib)]
        public void WriteFile(Thread thread, int operand1, int operand2)
        {
            if (fileContent is null)
            {
                thread.RuntimeError("no file is currently openned");
                return;
            }
            //append the char to the end of fileContent
            char c = (char)thread.Pop();
            fileContent += c;
        }

        /// <summary>
		/// readf <para/>
		/// read the char at cursor pos and advance the cursor
		/// </summary>
		[Instruction(0x64, "readf", lib)]
        public void Readfile(Thread thread, int operand1, int operand2)
        {
            if (fileContent is null)
            {
                thread.RuntimeError("no file is currently openned");
                return;
            }
            //read the char from the beginning of the fileContent and advance the cursor
            if (cursorPos >= fileContent.Length)
            {
                thread.RuntimeError("EOF reached");
                return;
            }
            char c = fileContent[cursorPos];
            cursorPos++;
            thread.Push(c);
        }

        /// <summary>
		/// setcur <para/>
		/// set the cursor position to tots
		/// </summary>
		[Instruction(0x65, "setcur", lib)]
        public void SetCursorPos(Thread thread, int operand1, int operand2)
        {
            //set the cursorPos to tots
            cursorPos = thread.Pop();
        }

        /// <summary>
		/// getcur <para/>
		/// get the cursor position
		/// </summary>
		[Instruction(0x66, "getcur", lib)]
        public void GetCursorPos(Thread thread, int operand1, int operand2)
        {
            //set the cursorPos to tots
            thread.Push(cursorPos);
        }

        /// <summary>
		/// lengthf <para/>
		/// get file content's length
		/// </summary>
		[Instruction(0x67, "lengthf", lib)]
        public void GetFileLength(Thread thread, int operand1, int operand2)
        {
            if (fileContent is null)
            {
                thread.RuntimeError("no file is currently openned");
                return;
            }
            //get fileContent's length
            thread.Push(fileContent.Length);
        }

        /// <summary>
		/// clearf <para/>
		/// clear the file content
		/// </summary>
		[Instruction(0x68, "clearf", lib)]
        public void ClearFile(Thread thread, int operand1, int operand2)
        {
            if (fileContent is null)
            {
                thread.RuntimeError("no file is currently openned");
                return;
            }
            //set fileContent to empty
            fileContent = string.Empty;
        }

        /// <summary>
		/// closef <para/>
		/// close the file
		/// </summary>
		[Instruction(0x69, "closef", lib)]
        public void CloseFile(Thread thread, int operand1, int operand2)
        {
            if (fileContent is null)
            {
                thread.RuntimeError("no file is currently openned");
                return;
            }
            //overwrite the file at filePath with fileContent
            File.WriteAllText(filePath, fileContent);

            filePath = null;
            fileContent = null;
        }
    }
}
