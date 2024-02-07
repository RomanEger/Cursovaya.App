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

        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            cb.IsDropDownOpen = true;
            var tb = (TextBox)e.OriginalSource;
            if (tb.SelectionStart != 0)
                cb.SelectedItem = null;
            else if (tb.SelectionStart == 0 && cb.SelectedItem == null)
                cb.IsDropDownOpen = false;

            cb.IsDropDownOpen = true;
            if(cb.SelectedItem == null)
            {
                CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(cb.ItemsSource);
                cv.Filter = s => ((string)s).IndexOf(cb.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
            }
        }
    }
}
