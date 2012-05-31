using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : TrayForm
    {
        private Class1 _class1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Launching...");

            _class1 = new Class1();
            _class1.Ping();

            uint size = 0;
            IntPtr handle = IntPtr.Zero;
            object stream = null;

            try
            {
                Guid guid = typeof(IClass1).GUID;
                int result = PInvoke.CoGetMarshalSizeMax(
                    out size,
                    ref guid,
                    _class1,
                    PInvoke.MSHCTX.LOCAL,
                    IntPtr.Zero,
                    PInvoke.MSHLFLAGS.NORMAL);

                Marshal.ThrowExceptionForHR(result);

                handle = PInvoke.GlobalAlloc(PInvoke.GMEM.MOVEABLE, size);

                result = PInvoke.CreateStreamOnHGlobal(handle, true, out stream);

                Marshal.ThrowExceptionForHR(result);

                result = PInvoke.CoMarshalInterface(
                    stream,
                    ref guid,
                    _class1,
                    PInvoke.MSHCTX.LOCAL,
                    IntPtr.Zero,
                    PInvoke.MSHLFLAGS.NORMAL);

                Marshal.ThrowExceptionForHR(result);
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Debug.WriteLine(ex.Message);
                    ex = ex.InnerException;
                }

                Marshal.ReleaseComObject(stream);
                PInvoke.GlobalFree(handle);
            }

            IntPtr ptr = PInvoke.GlobalLock(handle);

            string fileName = "C:\\DATA\\BUFFER.dat";

            if (File.Exists(fileName))
                File.Delete(fileName);

            using (FileStream fs = File.Create(fileName))
            {
                byte[] buf = new byte[size];
                Marshal.Copy(ptr, buf, 0, buf.Length);
                fs.Write(buf, 0, buf.Length);
            }

            Process process = new Process();
            process.StartInfo.FileName = "..\\..\\..\\..\\Win32ConsoleApplication11\\Debug\\Win32ConsoleApplication11.exe";
            Debug.WriteLine(process.StartInfo.Arguments);
            process.Start();

            PInvoke.GlobalUnlock(handle);
            Marshal.ReleaseComObject(stream);
        }
    }
}
