using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace VSIXPublishNUGET
{
    internal class OutWriter:TextWriter
    {


        public override Encoding Encoding => Encoding.UTF8;
        public override void Write(string s)
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate
            {
                var dte = Marshal.GetActiveObject("VisualStudio.DTE") as DTE2;
                var pane = dte.ToolWindows.OutputWindow.ActivePane;
                pane.OutputString(s);
            });

         }
        public override void WriteLine(string s)
        {
            Write($"{s}\r\n");
        }
    }
}
