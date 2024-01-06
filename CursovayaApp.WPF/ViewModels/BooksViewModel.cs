using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CursovayaApp.WPF.ViewModels
{
    public class BooksViewModel : ViewModelBase
    {
        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                Sort();
                OnPropertyChanged("SearchText");
            }
        }

        private void Sort()
        {
                var list = listBooks;
                if (!string.IsNullOrEmpty(SelectedAuthor) && SelectedAuthor != "Все")
                    list = list.Where(x => x.AuthorFullName == SelectedAuthor).ToList();
                list = list.Where(x => x.Title.ToLower().Contains(SearchText.ToLower())).ToList();
                Pagination.InsertToUsers(ref _books, list);
        }

        private List<BookView> listBooks;

        public ObservableCollection<string> Authors { get; set; }

        private string _selectedAuthor = string.Empty;

        public string SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                Sort();
                OnPropertyChanged("SelectedAuthor");
            }
        }

        private PaginationService<BookView> _pagination;

        public PaginationService<BookView> Pagination
        {
            get => _pagination;
            set
            {
                _pagination = value;
                OnPropertyChanged("Pagination");
            }
        }

        public BooksViewModel()
        {
            Pagination = new PaginationService<BookView>(10);
            try
            {
                GetData();
            }
            catch
            {

            }
        }
        private async void GetData()
        {
            GetBooks();
            GetAuthors();
        }
        private async Task GetBooksAsync()
        {
            var l= await (from book in DbClass.entities.Books
            join author in DbClass.entities.Authors
            on book.AuthorId equals author.Id
            join publishing in DbClass.entities.PublishingHouses
            on book.PublishingHouseId equals publishing.Id
            select new
            {
            Id=book.Id,
            Title=book.Title,
            AuthorFullName=author.FullName,
            Quantity=book.Quantity,
            Publishing=publishing.Name
            }).ToListAsync();
            //List<BookView> list = new List<BookView>();
            listBooks = new List<BookView>();
            foreach (var item in l)
            {
                listBooks.Add(new BookView()
                {
                    Id = item.Id,
                    Title=item.Title,
                    AuthorFullName=item.AuthorFullName,
                    Quantity=item.Quantity,
                    Publishing=item.Publishing
                });
            }

            Books = new ObservableCollection<BookView>();
            Pagination.InsertToUsers(ref _books, listBooks);
        }

        private void GetBooks()
        {
            var l = (from book in DbClass.entities.Books
                join author in DbClass.entities.Authors
                    on book.AuthorId equals author.Id
                join publishing in DbClass.entities.PublishingHouses
                    on book.PublishingHouseId equals publishing.Id
                select new
                {
                    Id = book.Id,
                    Title = book.Title,
                    AuthorFullName = author.FullName,
                    Quantity = book.Quantity,
                    Publishing = publishing.Name
                }).ToList();
            listBooks = new List<BookView>();
            foreach (var item in l)
            {
                listBooks.Add(new BookView()
                {
                    Id = item.Id,
                    Title = item.Title,
                    AuthorFullName = item.AuthorFullName,
                    Quantity = item.Quantity,
                    Publishing = item.Publishing
                });
            }

            Books = new ObservableCollection<BookView>();
            Pagination.InsertToUsers(ref _books, listBooks);
        }

        private void GetAuthors()
        {
            Authors = new ObservableCollection<string>(Books.ToList().Select(x => x.AuthorFullName).Distinct());
            Authors.Insert(0, "Все");
        }


        private RelayCommand _goBackCommand;

        public RelayCommand GoBackCommand
        {
            get
            {
                return _goBackCommand ??= new RelayCommand(obj =>
                {
                    if(MyFrame.frame.CanGoBack)
                        MyFrame.frame.GoBack();
                });
            }
        }

        private BookView _selectedBook;

        public BookView SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }

        private ICollection<BookView> _books;

        public ICollection<BookView> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged("Books");
            }
        }
    }
}
