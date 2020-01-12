using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace KancolleProgress
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            string dbPath = e.Args.Length switch
            {
                0 => Path.Combine(Directory.GetCurrentDirectory(), @"..\.."),
                _ => e.Args[0]
            };

            new MainWindow(dbPath).Show();
        }
    }
}
