using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClickeAlone
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte vk, byte scan, int flags, ref int extrainfo);
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        
        private const uint MOUSEMOVE = 0x0001; // 마우스 이동    
        private const uint ABSOLUTEMOVE = 0x8000; // 전역 위치    
        private const uint LBUTTONDOWN = 0x0002; // 왼쪽 마우스 버튼 눌림    
        private const uint LBUTTONUP = 0x0004;   // 왼쪽 마우스 버튼 떼어짐


        static bool IsRun { get; set; }

        public Form1()
        {
            InitializeComponent();

            IsRun = false;

            setComponentsEnabled(true);
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IsRun = true;

            setComponentsEnabled(false);

            int inteval = int.Parse(textBox1.Text);

            if (inteval < 100)
            {
                inteval = 100;
            }

            AsyncMouseClick(inteval);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IsRun = false;
            setComponentsEnabled(true);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void setComponentsEnabled(bool enabled)
        {
            textBox1.Enabled = enabled;
            button1.Enabled = enabled;
            button2.Enabled = !enabled;
        }

        static async void AsyncMouseClick(int inteval)
        {
            var task1 = Task.Run(() => MouseClicks(inteval));

            await task1;
        }

        static private void MouseClicks(int inteval)
        {   
            while (IsRun)
            {
                Thread.Sleep(inteval);

                mouse_event(LBUTTONDOWN, 0, 0, 0, 0); mouse_event(LBUTTONUP, 0, 0, 0, 0);
            }
        }
    }
}
