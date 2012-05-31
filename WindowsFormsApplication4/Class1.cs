using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication4
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
        public Class1()
        {
        }

        public void Ping()
        {
            Debug.WriteLine("M:{0}/U:{1} - Ping()", Thread.CurrentThread.ManagedThreadId, PInvoke.GetCurrentThreadId());

            //Application.OpenForms[0].BeginInvoke(
            //    new WaitCallback(
            //        delegate(object state1)
            //        {
            //            Debug.WriteLine("M:{0}/U:{1} - Ping()", Thread.CurrentThread.ManagedThreadId, PInvoke.GetCurrentThreadId());
            //        }),
            //    new object[] { null });
        }
    }
}
