using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
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
        private string separator = " | ";

        private readonly bool _forGive;

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
                if (!_forGive && _selectedClient != null)
                {
                    var arr = _selectedClient.Split(separator);
                    var clientId = DbClass.entities.Users.Where(x => x.FullName == arr[0] && x.Login == arr[1]).Select(x => x.Id).FirstOrDefault();
                    var books = from rental in DbClass.entities.RentalBooks
                                join user in DbClass.entities.Users
                                on rental.UserId equals user.Id
                                join book in DbClass.entities.Books
                                on rental.BookId equals book.Id
                                join author in DbClass.entities.Authors
                                on book.AuthorId equals author.Id
                                where user.Id == clientId
                                select book.Title + separator + author.FullName;
                    Books = new ObservableCollection<string>(books);
                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _books;

        public ObservableCollection<string> Books 
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Clients { get; set; }

        public RentalBookViewModel(bool forGive)
        {
            _forGive = forGive;
            if (forGive)
            {
                try
                {
                    var books = (from book in DbClass.entities.Books
                                join author in DbClass.entities.Authors
                                on book.AuthorId equals author.Id
                                select book.Title + separator + author.FullName).AsQueryable();
                    Books = new ObservableCollection<string>(books);
                    var clients = DbClass.entities.Users.Where(x => x.RoleId == 4).Select(x => x.FullName + separator + x.Login).AsQueryable();
                    Clients = new ObservableCollection<string>(clients);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    var clients = (from rental in DbClass.entities.RentalBooks
                                   join user in DbClass.entities.Users
                                   on rental.UserId equals user.Id
                                   select user.FullName + separator + user.Login).Distinct().AsQueryable();
                    Clients = new ObservableCollection<string>(clients);
                    Books = new();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        public RelayCommand GiveCommand => 
            new(obj =>
            {
                try
                {
                    var bookName = SelectedBook.Split(separator)[0];
                    var author = SelectedBook.Split(separator)[1];
                    var bookView = 
                                (from books in DbClass.entities.Books
                                join authors in DbClass.entities.Authors
                                on books.AuthorId equals authors.Id
                                select new
                                {
                                    books.Id,
                                    books.Title,
                                    authors.FullName,
                                }).FirstOrDefault(x => x.Title == bookName && x.FullName == author);

                    var canGive = Books.Count > DbClass.entities.RentalBooks.Where(x => x.IsRentalEnd == false && x.BookId == bookView.Id).Count();
                    if (!canGive)
                    {
                        MessageBox.Show("Нет доступных книг!");
                        return;
                    }

                    var clientFullName = SelectedClient.Split(separator)[0];

                    var clientLogin = SelectedClient.Split(separator)[1];

                    var client = DbClass.entities.Users.FirstOrDefault(x => x.RoleId == 4 && x.FullName == clientFullName && x.Login == clientLogin);

                    var entitie = new RentalBook()
                    {
                        BookId = bookView.Id,
                        UserId = client.Id,
                        DateStart = DateTime.Now.Date,
                        DateEnd = DateTime.Now.AddDays(14).Date,
                        IsRentalEnd = false
                    };
                    DbClass.entities.RentalBooks.Add(entitie);
                    DbClass.entities.SaveChanges();
                    MessageBox.Show("Книга выдана! Изменения сохранены!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

        public RelayCommand RecieveCommand =>
            new(obj =>
            {
                try
                {
                    var bookName = SelectedBook.Split(separator)[0];
                    var author = SelectedBook.Split(separator)[1];
                    var bookView =
                                (from books in DbClass.entities.Books
                                 join authors in DbClass.entities.Authors
                                 on books.AuthorId equals authors.Id
                                 select new
                                 {
                                     books.Id,
                                     books.Title,
                                     authors.FullName,
                                 }).FirstOrDefault(x => x.Title == bookName && x.FullName == author);

                    var clientFullName = SelectedClient.Split(separator)[0];

                    var clientLogin = SelectedClient.Split(separator)[1];

                    var client = DbClass.entities.Users.FirstOrDefault(x => x.RoleId == 4 && x.FullName == clientFullName && x.Login == clientLogin);

                    var entitie = DbClass.entities.RentalBooks.FirstOrDefault(x => x.BookId == bookView.Id && x.UserId == client.Id);

                    entitie.IsRentalEnd = true;

                    DbClass.entities.SaveChanges();

                    MessageBox.Show("Книга принята! Изменения сохранены!");
                }
                catch (Exception ex )
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
    }
}
