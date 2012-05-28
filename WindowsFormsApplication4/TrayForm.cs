using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class TrayForm : Form
    {
        private enum ToggleMode { Show, Hide };
        private FormWindowState _previousState;
        private bool _contextMenuVisible = false;

        public TrayForm()
        {
            InitializeComponent();

            Font = new Font("MS Shell Dlg 2", Font.Size);

            if (DesignMode || Process.GetCurrentProcess().ProcessName == "devenv")
            {
                notifyIcon.Visible = false;
            }
            else
            {
                StartPosition = FormStartPosition.CenterScreen;

                Load += new EventHandler(TrayForm_Load);
                SizeChanged += new EventHandler(TrayForm_SizeChanged);
                FormClosing += new FormClosingEventHandler(TrayForm_FormClosing);

                ContextMenu contextMenu = new ContextMenu(
                    new MenuItem[]
                {
                    new MenuItem(String.Empty, new EventHandler(ToggleForm)),
                    new MenuItem("&Quit", new EventHandler(Quit))
                }
                );

                contextMenu.MenuItems[0].DefaultItem = true;
                contextMenu.Popup += new EventHandler(contextMenu_Popup);
                //contextMenu.MenuItems[0].Tag = ToggleMode.Show;

                notifyIcon.ContextMenu = contextMenu;
                notifyIcon.Click += new EventHandler(notifyIcon_Click);
                notifyIcon.DoubleClick += new EventHandler(contextMenu_Popup);

                ThreadPool.QueueUserWorkItem(
                    new WaitCallback(
                        delegate(object state1)
                        {
                            Thread.Sleep(500);

                            BeginInvoke(
                                new WaitCallback(
                                    delegate(object state2)
                                    {
                                        WindowState = FormWindowState.Minimized;

                                        notifyIcon.ShowBalloonTip(4000, "WindowsFormsApplication4", "Still running...", ToolTipIcon.Info);
                                    }),
                                new object[] { null });
                        }),
                    null);
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            if (!_contextMenuVisible)
            {
                bool visible = Visible;
                if (!visible)
                    Visible = true;

                notifyIcon.ContextMenu.Show(this, PointToClient(Cursor.Position));

                if (!visible)
                    visible = false;
            }
        }

        private void contextMenu_Popup(object sender, EventArgs e)
        {
            ContextMenu contextMenu = null;

            if (sender is ContextMenu)
            {
                contextMenu = (ContextMenu)sender;
                _contextMenuVisible = true;
            }
            else if (sender is NotifyIcon)
                contextMenu = ((NotifyIcon)sender).ContextMenu;

            if (contextMenu != null)
            {
                contextMenu.MenuItems[0].Text = WindowState == FormWindowState.Minimized ? "&Show" : "&Hide";
                contextMenu.MenuItems[0].Tag = WindowState == FormWindowState.Minimized ? ToggleMode.Show : ToggleMode.Hide;
            }

            if (sender is NotifyIcon)
                ToggleForm(sender, new EventArgs());
        }

        private void TrayForm_Load(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Minimized;
        }

        private void TrayForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                TrayForm_AfterMinimized(sender, e);
        }

        private void TrayForm_BeforeMinimized(object sender, EventArgs e)
        {
            _previousState = WindowState;
        }

        private void TrayForm_AfterMinimized(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(
                new WaitCallback(
                    delegate(object state1)
                    {
                        Thread.Sleep(250);

                        BeginInvoke(
                            new WaitCallback(
                                delegate(object state2)
                                {
                                    Visible = false;
                                }
                            ),
                            new object[] { null }
                        );
                    }
                )
            );
        }

        private void TrayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;

                _previousState = WindowState;
                WindowState = FormWindowState.Minimized;
            }
        }

        private void ToggleForm(object sender, EventArgs e)
        {
            _contextMenuVisible = false;

            switch ((ToggleMode)notifyIcon.ContextMenu.MenuItems[0].Tag)
            {
                case ToggleMode.Show:
                    Visible = true;

                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(
                            delegate(object state1)
                            {
                                Thread.Sleep(250);

                                BeginInvoke(
                                    new WaitCallback(
                                        delegate(object state2)
                                        {
                                            WindowState = _previousState;
                                        }
                                    ),
                                    new object[] { null }
                                );
                            }
                        ),
                        null
                    );
                    break;
                case ToggleMode.Hide:
                    TrayForm_BeforeMinimized(this, new EventArgs());
                    WindowState = FormWindowState.Minimized;
                    break;
            }
        }

        private void Quit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0112: // WM_SYSCOMMAND
                    if ((m.WParam.ToInt32() & 0xfff0) == 0xf020) // SC_MINIMIZE
                        TrayForm_BeforeMinimized(this, new EventArgs());

                    break;
            }

            base.WndProc(ref m);
        }
    }
}
