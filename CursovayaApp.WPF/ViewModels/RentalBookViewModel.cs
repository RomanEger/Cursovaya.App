using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository;
using CursovayaApp.WPF.Repository.Contracts;
using CursovayaApp.WPF.Services;
using Microsoft.EntityFrameworkCore;
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
        private readonly IGenericRepository<RentalBook> _repositoryRentalBook;
        private readonly IGenericRepository<Book> _repositoryBook;
        private readonly IGenericRepository<Author> _repositoryAuthor;
        private readonly IGenericRepository<User> _repositoryUser;

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
                    var clientId = _repositoryUser.Where(x => x.FullName == arr[0] && x.Login == arr[1]).Select(x => x.Id).FirstOrDefault();
                    var books = from rental in _repositoryRentalBook.GetAll()
                                join user in _repositoryUser.GetAll()
                                on rental.UserId equals user.Id
                                join book in _repositoryBook.GetAll()
                                on rental.BookId equals book.Id
                                join author in _repositoryAuthor.GetAll()
                                on book.AuthorId equals author.Id
                                where user.Id == clientId && rental.IsRentalEnd == false
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
            
            _repositoryRentalBook = new GenericRepository<RentalBook>(new ApplicationContext());
            _repositoryBook = new GenericRepository<Book>(new ApplicationContext());
            _repositoryAuthor = new GenericRepository<Author>(new ApplicationContext());
            _repositoryUser = new GenericRepository<User>(new ApplicationContext());

            if (forGive)
            {
                try
                {
                    var books = (from book in _repositoryBook.GetAll()
                                join author in _repositoryAuthor.GetAll()
                                on book.AuthorId equals author.Id
                                select book.Title + separator + author.FullName).AsQueryable();
                    Books = new ObservableCollection<string>(books.ToList());
                    var clients = _repositoryUser.Where(x => x.RoleId == 4).Select(x => x.FullName + separator + x.Login).AsQueryable();
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
                    var clients = (from rental in _repositoryRentalBook.GetAll()
                                   join user in _repositoryUser.GetAll()
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
                                (from books in _repositoryBook.GetAll()
                                join authors in _repositoryAuthor.GetAll()
                                on books.AuthorId equals authors.Id
                                select new
                                {
                                    books.Id,
                                    books.Title,
                                    authors.FullName,
                                }).FirstOrDefault(x => x.Title == bookName && x.FullName == author);

                    var count = _repositoryRentalBook.Count(x =>
                        x.IsRentalEnd == false && x.BookId == bookView.Id);
                    var canGive = Books.Count > count;
                    if (!canGive)
                    {
                        MessageBox.Show("Нет доступных книг!");
                        return;
                    }

                    var clientFullName = SelectedClient.Split(separator)[0];

                    var clientLogin = SelectedClient.Split(separator)[1];

                    var client = _repositoryUser.Get(x => x.RoleId == 4 && x.FullName == clientFullName && x.Login == clientLogin);

                    var entitie = new RentalBook()
                    {
                        BookId = bookView.Id,
                        UserId = client.Id,
                        DateStart = DateTime.Now.Date,
                        DateEnd = DateTime.Now.AddDays(14).Date,
                        IsRentalEnd = false
                    };
                    _repositoryRentalBook.Add(entitie);
                    _repositoryRentalBook.Save();
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
                                (from books in _repositoryBook.GetAll()
                                 join authors in _repositoryAuthor.GetAll()
                                 on books.AuthorId equals authors.Id
                                 select new
                                 {
                                     books.Id,
                                     books.Title,
                                     authors.FullName,
                                 }).FirstOrDefault(x => x.Title == bookName && x.FullName == author);

                    var clientFullName = SelectedClient.Split(separator)[0];

                    var clientLogin = SelectedClient.Split(separator)[1];

                    var client = _repositoryUser.Get(x => x.RoleId == 4 && x.FullName == clientFullName && x.Login == clientLogin);

                    var entitie = _repositoryRentalBook.Get(x => x.BookId == bookView.Id && x.UserId == client.Id);

                    entitie.IsRentalEnd = true;
                    
                    _repositoryRentalBook.Update(entitie);

                    MessageBox.Show("Книга принята! Изменения сохранены!");
                    SelectedBook = null;
                    SelectedClient = SelectedClient;
                }
                catch (Exception ex )
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
    }
}
