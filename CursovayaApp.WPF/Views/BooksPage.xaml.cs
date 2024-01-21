using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using CursovayaApp.WPF.ViewModels;

namespace CursovayaApp.WPF.Views
{
    /// <summary>
    /// Логика взаимодействия для BooksPage.xaml
    /// </summary>
    public partial class BooksPage : Page
    {
        public BooksPage()
        {
            InitializeComponent();
            DataContext = new BooksViewModel();
        }


        private void BooksPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoggedUser loggedUser = new();
            if (loggedUser.CurrentUser.RoleId == 1)//админ
            {
                StackPanelForStock.Visibility = Visibility.Collapsed;
                StackPanelForLibr.Visibility = Visibility.Collapsed;
            }
            else if (loggedUser.CurrentUser.RoleId == 2)//библиотекарь
            {
                StackPanelForLibr.Visibility = Visibility.Visible;
                StackPanelForStock.Visibility = Visibility.Collapsed;
            }
            else if (loggedUser.CurrentUser.RoleId == 3)//кладовщик
            {
                StackPanelForStock.Visibility = Visibility.Visible;
                StackPanelForLibr.Visibility = Visibility.Collapsed;
            }
        }
    }
}
