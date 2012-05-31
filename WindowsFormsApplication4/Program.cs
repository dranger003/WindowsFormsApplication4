using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Debug.WriteLine("M:{0}/U:{1} - Main()", Thread.CurrentThread.ManagedThreadId, PInvoke.GetCurrentThreadId());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //if (args.Length > 0)
            //    MessageBox.Show(args[0]);

            if (args.Length == 1 && args[0] == "-Embedding")
            {
                MessageBox.Show(args[0]);
                return;
            }

            RegistrationServices rs = new RegistrationServices();
            int cookie = rs.RegisterTypeForComClients(typeof(Class1), RegistrationClassContext.LocalServer, RegistrationConnectionType.MultipleUse);

            try
            {
                Application.Run(new Form1());
            }
            catch
            {
            }

            rs.UnregisterTypeForComClients(cookie);
        }
    }
}
