using System;
using System.Windows.Forms;
using BaiTap5_4.GUI; // <-- Thêm dòng này vào

namespace BaiTap5_4
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}