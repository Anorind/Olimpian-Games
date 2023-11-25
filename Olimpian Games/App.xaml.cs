using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Olimpian_Games
{
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            var application = new App();
            var mainWindow = new MainWindow();
            application.Run(mainWindow);

        }

    }

}

