using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

using XIL.LangDef;
using XIL.VM;

namespace XIL.StandardLibrary
{
    public class HTTPInstruction : IInstructionImplementation
    {
        const string lib = "http";

        public HttpClient HttpClient = new HttpClient();

        public void t()
        {
            HttpClient.GetStringAsync("");
            HttpClient.PostAsync("", null);
            HttpClient.DefaultRequestHeaders.Add("", "");

        }

        /// <summary>
        /// pusho <para/>
        /// push an empty object on stack
        /// </summary>
        [Instruction(0xA0, "http_setaddr", lib)]
        public void SetBaseAddr(Thread thread, int op1, int op2)
        {
            var baseAddr = Util.PopStringFromStack(thread);
            HttpClient.BaseAddress = new Uri(baseAddr);
        }

        /// <summary>
        /// pusho <para/>
        /// push an empty object on stack
        /// </summary>
        [Instruction(0xA1, "http_post", lib)]
        public void Post(Thread thread, int op1, int op2)
        {
            Dictionary<string, string> form = Util.PopObject(thread);
            using (StringContent content = new StringContent(JsonSerializer.Serialize(form), Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage httpResult = HttpClient.PostAsync(string.Empty, content).Result;
                string result = httpResult.Content.ReadAsStringAsync().Result;
                Util.PushStringToStack(thread, result);
            }
        }
    }
}
