using CursovayaApp.WPF.Models;
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

                //bindingIS.ElementName = "cmb";
                bindingIS.Path = new PropertyPath("AuthorsForAdd");
                cmb.SetBinding(ComboBox.ItemsSourceProperty, bindingIS);
                //bindingSI.ElementName = "cmb";
                bindingSI.Path = new PropertyPath("SelectedAuthor");
                cmb.SetBinding(ComboBox.SelectedItemProperty, bindingSI);
                //bindingC.ElementName = "btn";
                bindingC.Path = new PropertyPath("AddAuthorCommand");
                btn.SetBinding(Button.CommandProperty, bindingC);
            }
            else
            {
                SetPublishings();

                //bindingIS.ElementName = "cmb";
                bindingIS.Path = new PropertyPath("ListPublishings");
                cmb.SetBinding(ComboBox.ItemsSourceProperty, bindingIS);
                //bindingSI.ElementName = "cmb";
                bindingSI.Path = new PropertyPath("SelectedPublishing");
                cmb.SetBinding(ComboBox.SelectedItemProperty, bindingSI);
                //bindingC.ElementName = "btn";
                bindingC.Path = new PropertyPath("AddPublishingCommand");
                btn.SetBinding(Button.CommandProperty, bindingC);
            }
        }
        private void SetPublishings()
        {
            try
            {
                if (_vm.ListPublishings != null && _vm.ListPublishings.Any()) return;

                List<string> list = DbClass.entities.PublishingHouses.Select(x => x.Name).ToList();
                _vm.ListPublishings = new System.Collections.ObjectModel.ObservableCollection<string>(list);
            }
            catch
            {
                MessageBox.Show("Не удалось получить список издательств!");
            }
        }

        private void SetAuthors()
        {
            try
            {
                if (_vm.AuthorsForAdd != null && _vm.AuthorsForAdd.Any()) return;

                List<string> list = DbClass.entities.Authors.Select(x => x.FullName).ToList();
                _vm.AuthorsForAdd = new System.Collections.ObjectModel.ObservableCollection<string>(list);
            }
            catch
            {
                MessageBox.Show("Не удалось получить список авторов!");
            }
        }
    }
}
