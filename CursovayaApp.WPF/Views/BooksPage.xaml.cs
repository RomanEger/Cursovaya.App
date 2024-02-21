using System.Windows;
using System.Windows.Controls;
using CursovayaApp.WPF.Models;
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
                StackPanelForAdmin.Visibility = Visibility.Visible;
                StackPanelForStock.Visibility = Visibility.Collapsed;
                StackPanelForLibr.Visibility = Visibility.Collapsed;
            }
            else if (loggedUser.CurrentUser.RoleId == 2)//библиотекарь
            {
                StackPanelForLibr.Visibility = Visibility.Visible;
                StackPanelForStock.Visibility = Visibility.Collapsed;
                StackPanelForAdmin.Visibility = Visibility.Collapsed;
            }
            else if (loggedUser.CurrentUser.RoleId == 3)//кладовщик
            {
                StackPanelForStock.Visibility = Visibility.Visible;
                StackPanelForLibr.Visibility = Visibility.Collapsed;
                StackPanelForAdmin.Visibility = Visibility.Collapsed;
            }
            else//клиент, если будет
            {
                StackPanelForStock.Visibility = Visibility.Collapsed;
                StackPanelForLibr.Visibility = Visibility.Collapsed;
                StackPanelForAdmin.Visibility = Visibility.Collapsed;
            }
        }
    }
}
