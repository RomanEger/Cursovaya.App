using System.Windows.Controls;
using CursovayaApp.WPF.ViewModels;

namespace CursovayaApp.WPF.Views
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            DataContext = new AdminViewModel();
        }
    }
}
