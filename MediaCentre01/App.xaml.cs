﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MediaCentre01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(Object sender, StartupEventArgs e)
        {
            //MainWindow wnd = new MainWindow();

            var wnd = new Pomo();
            wnd.Show();
        }

    }
}
