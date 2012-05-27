using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using ClassLibrary1;

namespace WindowsFormsApplication4
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RegistrationServices rs = new RegistrationServices();
            int cookie = rs.RegisterTypeForComClients(typeof(Class1), RegistrationClassContext.LocalServer, RegistrationConnectionType.MultipleUse);

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch
            {
            }

            rs.UnregisterTypeForComClients(cookie);
        }
    }
}
