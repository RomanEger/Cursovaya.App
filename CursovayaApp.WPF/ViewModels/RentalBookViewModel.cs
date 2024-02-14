using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CursovayaApp.WPF.ViewModels
{
    public class RentalBookViewModel : ViewModelBase
    {
        private string _selectedBook;
        public string SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }

        private string _selectedClient;

        public string SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Books { get; set; }

        public ObservableCollection<string> Clients { get; set; }

        public RentalBookViewModel()
        {
            var books = (from book in DbClass.entities.Books
                        join author in DbClass.entities.Authors
                        on book.AuthorId equals author.Id
                        select book.Title + " " + author.FullName).AsQueryable();
            Books = new ObservableCollection<string>(books);
            var clients = DbClass.entities.Users.Where(x => x.RoleId == 4).Select(x => x.FullName + " " + x.Login).AsQueryable();
            Clients = new ObservableCollection<string>(clients);
        }
    }
}
