﻿using System;
using System.Windows.Forms;

namespace SokobanBeta
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Log_In()); // Chạy form Menu
        }
    }
}
