using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;

namespace CursovayaApp.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginPage l;
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
            MyFrame.frame = MainFrame;
            Title = "Библиотека \"Читайка\"";
            l = new LoginPage();
            MyFrame.Navigate(l);
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            var x = MyFrame.frame.Content as Page;
            if(l.GetType() == x?.GetType())
                return;

            var a = MessageBox.Show(
                "Хотите ли вы сохранить изменения?", "",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Asterisk);
            if (a == MessageBoxResult.Yes)
            {
                try
                {
                    var repo = new ApplicationContext();
                    repo.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (a == MessageBoxResult.Cancel) 
            {
                e.Cancel = true;
            }
        }
    }
}