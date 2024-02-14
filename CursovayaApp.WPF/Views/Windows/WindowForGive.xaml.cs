using CursovayaApp.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CursovayaApp.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ForLibrPage.xaml
    /// </summary>
    public partial class WindowForGive : Window
    {
        public WindowForGive()
        {
            InitializeComponent();
            var vm = new RentalBookViewModel();
            DataContext = vm;
        }

        private void CmbClient_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CmbClients.IsDropDownOpen = true;
            var tb = (TextBox)e.OriginalSource;
            if (tb.SelectionStart != 0)
                CmbClients.SelectedItem = null;
            else if (tb.SelectionStart == 0 && CmbClients.SelectedItem == null)
                CmbClients.IsDropDownOpen = false;

            CmbClients.IsDropDownOpen = true;
            if (CmbClients.SelectedItem == null)
            {
                CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(CmbClients.ItemsSource);
                cv.Filter = s => ((string)s).IndexOf(CmbClients.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
            }
        }

        private void CmbBooks_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CmbBooks.IsDropDownOpen = true;
            var tb = (TextBox)e.OriginalSource;
            if (tb.SelectionStart != 0)
                CmbBooks.SelectedItem = null;
            else if (tb.SelectionStart == 0 && CmbBooks.SelectedItem == null)
                CmbBooks.IsDropDownOpen = false;

            CmbBooks.IsDropDownOpen = true;
            if (CmbBooks.SelectedItem == null)
            {
                CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(CmbBooks.ItemsSource);
                cv.Filter = s => ((string)s).IndexOf(CmbBooks.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
            }
        }
    }
}
