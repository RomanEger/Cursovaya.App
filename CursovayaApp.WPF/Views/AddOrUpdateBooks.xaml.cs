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
using System.Windows.Shapes;
using CursovayaApp.WPF.ViewModels;

namespace CursovayaApp.WPF.Views
{
    /// <summary>
    /// Логика взаимодействия для AddOrUpdateBooks.xaml
    /// </summary>
    public partial class AddOrUpdateBooks : Window
    {
        public AddOrUpdateBooks(BooksViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
