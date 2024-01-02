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
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode =  ResizeMode.CanMinimize;
            MyFrame.frame = MainFrame;
            var l = new LoginPage();
            MyFrame.Navigate(l);
            DbClass.entities = new ApplicationContext();
            Title = "Библиотека \"Читайка\"";
        }
    }
}