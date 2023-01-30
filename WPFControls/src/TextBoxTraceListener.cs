using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sabatex.WPF.Controls.Diagnostics
{
    public class TextBoxTraceListener: TraceListener
    {
        public TextBoxTraceListener(TextBox target)
        {
            Target = target;
            InvokeWrite = new StringSendDelegate(SendString);
        }
        public override void Write(string message)
        {
            Target.Dispatcher.Invoke(InvokeWrite, new object[] { message });
        }

        public override void WriteLine(string message)
        {
            Target.Dispatcher.Invoke(InvokeWrite, new object[] { DateTime.Now.ToString() + " " + message + Environment.NewLine });
        }
        private void SendString(string message)
        {
            // No need to lock text box as this function will only
            // ever be executed from the UI thread
            Target.AppendText(message);
            Target.ScrollToEnd();
        }


        private TextBox Target;
        private StringSendDelegate InvokeWrite;
        private delegate void StringSendDelegate(string message);
    }
}
