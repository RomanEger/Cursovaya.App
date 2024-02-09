using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.ViewModels;
using Microsoft.EntityFrameworkCore;
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

namespace CursovayaApp.WPF.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ChoosePublishing.xaml
    /// </summary>
    public partial class ChoosePublishing : Window
    {
        BooksViewModel _vm;
        public ChoosePublishing(BooksViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
            if (_vm.ListPublishings != null)
                return;
            SetPublishings();
        }
        private async Task SetPublishings()
        {
            try
            {
                var list = await DbClass.entities.PublishingHouses.Select(x => x.Name).ToListAsync();
                _vm.ListPublishings = new System.Collections.ObjectModel.ObservableCollection<string>(list);
            }
            catch
            {
                MessageBox.Show("Не удалось получить список издательств!");
            }
        }
    }
}
