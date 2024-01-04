using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.ViewModels;
using CursovayaApp.WPF.Views;
using Microsoft.EntityFrameworkCore;

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
            ResizeMode =  ResizeMode.CanMinimize;
            DbClass.entities = new ApplicationContext();
            MyFrame.frame = MainFrame;
            Title = "Библиотека \"Читайка\"";
            l = new LoginPage();
            MyFrame.Navigate(l);
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            var x = MyFrame.frame.Content as Page;
            if(x == l)
                return;

            var a = MessageBox.Show(
                "Хотите ли вы сохранить изменения?", "",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Asterisk);
            if (a == MessageBoxResult.Yes)
            {
                try
                {
                    DbClass.entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                    FileStream fileStream = new FileStream(fileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fileStream);
                    sw.Write(ex.Message);
                    sw.Close();
                }
            }
            else if (a == MessageBoxResult.Cancel) 
            {
                e.Cancel = true;
            }
        }
    }
}