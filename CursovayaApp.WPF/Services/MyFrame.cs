using System.Windows.Controls;

namespace CursovayaApp.WPF.Services
{
    internal static class MyFrame
    {
        public static Frame Frame;

        public static bool Navigate(Page newPage) =>
            Frame?.Navigate(newPage) ?? false;

        public static void ClearHistory()
        {
            while (Frame is { CanGoBack: true, CanGoForward: true })
            {
                try
                {
                    Frame.RemoveBackEntry();
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
    }
}
