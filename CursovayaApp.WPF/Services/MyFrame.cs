using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CursovayaApp.WPF.Services
{
    internal class MyFrame
    {
        public static Frame frame;

        public static bool Navigate(Page newPage)
        {
            return frame?.Navigate(newPage) ?? false;
        }
    }
}
