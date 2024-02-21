using System.Windows;
using System.Windows.Controls;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.ViewModels;

namespace CursovayaApp.WPF.Views.Windows;

public partial class AddOrUpdateAuthors : Window
{
    public AddOrUpdateAuthors(Author author, BooksViewModel vm)
    {
        InitializeComponent();
        DataContext = new AuthorViewModel(author, vm);
        MinHeight = 260;
        MinWidth = 500;
    }

    private void CheckBox_Click(object sender, RoutedEventArgs e)
    {
        dpDeath.IsEnabled = sender is not CheckBox { IsChecked: true };
    }
}