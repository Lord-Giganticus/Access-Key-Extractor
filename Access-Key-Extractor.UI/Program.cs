﻿using Eto.Drawing;
using Eto.Forms;
using System;

namespace Access_Key_Extractor.UI
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Application(Eto.Platform.Detect).Run(new MainForm());
        }
    }
}
