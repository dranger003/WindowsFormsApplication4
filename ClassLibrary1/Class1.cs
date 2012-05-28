using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClassLibrary1
{
    [ComVisible(true)]
    [Guid("06746A9C-963E-4570-8550-23C18B445DC9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IClass1
    {
        void Ping();
    }

    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Guid("96575AAA-F6D2-4879-9003-E4AD9CD40C76")]
    public class Class1 : IClass1
    {
        private static Form _form;

        public Class1()
        {
        }

        public void Ping()
        {
            Debug.WriteLine("Ping()");
        }

        public Form Form
        {
            get { return _form; }
            set { _form = value; }
        }
    }
}
