using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.ViewModels;
using System.Windows;

namespace CursovayaApp.WPF.Views
{
    /// <summary>
    /// Логика взаимодействия для AddOrUpdatePublishings.xaml
    /// </summary>
    public partial class AddOrUpdatePublishings : Window
    {
        public AddOrUpdatePublishings(PublishingHouse publishingHouse, BooksViewModel vm)
        {
            InitializeComponent();
            var publishingVM = new PublishingViewModel(publishingHouse, vm);
            DataContext = publishingVM;
        }
    }
}
