using CursovayaApp.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace CursovayaApp.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ForLibrPage.xaml
    /// </summary>
    public partial class WindowForLibr : Window
    {
        public WindowForLibr(bool forGive)
        {
            var vm = new RentalBookViewModel(forGive);
            DataContext = vm;
            InitializeComponent();
            Binding binding = new()
            {
                Source = DataContext
            };
            if (forGive)
            {
                btn.Content = "Выдать";
                binding.Path = new PropertyPath("GiveCommand");
                btn.SetBinding(ButtonBase.CommandProperty, binding);
            }
            else
            {
                btn.Content = "Принять";
                binding.Path = new PropertyPath("RecieveCommand");
                btn.SetBinding(ButtonBase.CommandProperty, binding);
            }
        }

        private void CmbClient_TextChanged(object sender, TextChangedEventArgs e)
        {
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

        private void CmbBooks_TextChanged(object sender, TextChangedEventArgs e)
        {
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
