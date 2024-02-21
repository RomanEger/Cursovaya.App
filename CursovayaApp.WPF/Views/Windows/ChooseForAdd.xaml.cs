using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository;
using CursovayaApp.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CursovayaApp.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChoosePublishing.xaml
    /// </summary>
    public partial class ChooseForAdd : Window
    {
        private readonly BooksViewModel _vm;
        public ChooseForAdd(BooksViewModel vm, bool isAuthor)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            Binding bindingIS = new() { Source = DataContext };
            Binding bindingSI = new() { Source = DataContext };
            Binding bindingC = new() { Source = DataContext };

            if (isAuthor)
            {
                SetAuthors();

                bindingIS.Path = new PropertyPath("AuthorsForAdd");
                cmb.SetBinding(ComboBox.ItemsSourceProperty, bindingIS);
                bindingSI.Path = new PropertyPath("SelectedAuthor");
                cmb.SetBinding(ComboBox.SelectedItemProperty, bindingSI);
                bindingC.Path = new PropertyPath("AddAuthorCommand");
                btn.SetBinding(Button.CommandProperty, bindingC);
            }
            else
            {
                SetPublishings();

                bindingIS.Path = new PropertyPath("ListPublishings");
                cmb.SetBinding(ComboBox.ItemsSourceProperty, bindingIS);
                bindingSI.Path = new PropertyPath("SelectedPublishing");
                cmb.SetBinding(ComboBox.SelectedItemProperty, bindingSI);
                bindingC.Path = new PropertyPath("AddPublishingCommand");
                btn.SetBinding(Button.CommandProperty, bindingC);
            }
        }
        private void SetPublishings()
        {
            try
            {
                var repo = new GenericRepository<PublishingHouse>(new ApplicationContext());

                if (_vm.ListPublishings != null && _vm.ListPublishings.Any()) return;

                List<string> list = repo.GetAll().Select(x => x.Name).ToList();
                _vm.ListPublishings = new System.Collections.ObjectModel.ObservableCollection<string>(list);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Не удалось получить список издательств!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetAuthors()
        {
            try
            {
                var repo = new GenericRepository<Author>(new ApplicationContext());

                if (_vm.AuthorsForAdd != null && _vm.AuthorsForAdd.Any()) return;

                List<string> list = repo.GetAll().Select(x => x.FullName).ToList();
                _vm.AuthorsForAdd = new System.Collections.ObjectModel.ObservableCollection<string>(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Не удалось получить список авторов!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
