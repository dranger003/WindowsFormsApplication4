using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ClassLibrary1;

namespace WindowsFormsApplication4
{
    public partial class Form1 : TrayForm
    {
        private Class1 _class1 = new Class1();

        public Form1()
        {
            InitializeComponent();

            _class1.Form = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}
