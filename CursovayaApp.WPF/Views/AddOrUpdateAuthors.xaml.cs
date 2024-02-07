using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.ViewModels;

namespace CursovayaApp.WPF.Views;

public partial class AddOrUpdateAuthors : Window
{
    public AddOrUpdateAuthors(Author author, BooksViewModel vm)
    {
        InitializeComponent();
        DataContext = new AuthorViewModel(author, vm);
    }

    private void CheckBox_Click(object sender, RoutedEventArgs e)
    {
        var s = sender as CheckBox;
        if (s.IsChecked == true)
            dpDeath.IsEnabled = false;
        else
            dpDeath.IsEnabled = true;
    }
}