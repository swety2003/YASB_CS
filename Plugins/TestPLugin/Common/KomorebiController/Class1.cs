using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin.KomorebiController
{
    public static class CommandSender
    {
        public static void StartProcess()
        {

        }

        public static void StopProcess() { }


        private static void CallKomorebic(string args) 
        {
            Process process = new Process();
            process.StartInfo.FileName = "komorebic.exe";
            process.StartInfo.Arguments =args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }

        public static void Subscribe(string pipeName)
        {
            CallKomorebic($"subscribe {pipeName}");
        }

        public static void UnSubscribe(string pipeName = PipeServer.pipeName)
        {
            CallKomorebic($"unsubscribe {pipeName}");
        }

        internal static void ChangeWorkSpace(string name = PipeServer.pipeName)
        {
            CallKomorebic($"focus-named-workspace {name}");
        }
    }

    public class PipeServer
    {
        public const string pipeName = "testpipe233";

        public NamedPipeServerStream? pipeServer;
        public void Create()
        {
            pipeServer =
                new NamedPipeServerStream(pipeName, PipeDirection.InOut);
            Console.WriteLine("Server started, waiting for connection.");
        }


    }





}
