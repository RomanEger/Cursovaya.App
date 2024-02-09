using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CursovayaApp.WPF.Services
{
    internal class MyFrame
    {
        public static Frame frame;

        public static bool Navigate(Page newPage) =>
            frame?.Navigate(newPage) ?? false;

        public static void ClearHistory()
        {
            while (frame.CanGoBack && frame.CanGoForward)
            {
                try
                {
                    frame.RemoveBackEntry();
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }
    }
}
