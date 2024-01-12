using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private List<BookView> listBooks;

        private List<BookView> sortedListBooks;

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
        
        private void Sort()
        {
                sortedListBooks = listBooks;
                if (!string.IsNullOrEmpty(SelectedAuthor) && SelectedAuthor != "Все")
                    sortedListBooks = sortedListBooks.Where(x => x.AuthorFullName == SelectedAuthor).ToList();
                sortedListBooks = sortedListBooks.Where(x => x.Title.ToLower().Contains(SearchText.ToLower())).ToList();
                Pagination.InsertToUsers(ref _books, sortedListBooks);
        }

        private void SetCount()
        {
            Pagination.Count = (int)Math.Ceiling(sortedListBooks.Count * 1.0 / Pagination.TsAtPage);
        }

        private async void GetData()
        {
            GetBooks();
            GetAuthors();
            SetCount();
        }

        private async Task GetBooksAsync()
        {
            var l= 
                await (from book in DbClass.entities.Books
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

            sortedListBooks = listBooks;
            Books = new ObservableCollection<BookView>();
            Pagination.InsertToUsers(ref _books, listBooks);
        }

        private void GetBooks()
        {
            var l = 
                (from book in DbClass.entities.Books
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

            sortedListBooks = listBooks;
            Books = new ObservableCollection<BookView>();
            Pagination.InsertToUsers(ref _books, listBooks);
        }

        private void GetAuthors()
        {
            Authors = new ObservableCollection<string>(listBooks.Select(x => x.AuthorFullName).Distinct());
            Authors.Insert(0, "Все");
        }

        public BooksViewModel()
        {
            Pagination = new PaginationService<BookView>(1);
            try
            {
                GetData();
            }
            catch
            {

            }
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

        private RelayCommand _firstUsersCommand;
        public RelayCommand FirstUsersCommand
        {
            get
            {
                return _firstUsersCommand ??= new RelayCommand(obj =>
                {
                    Pagination.FirstT(ref _books, listBooks);
                });
            }
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var a = DbClass.entities.Books.ToList();
                        foreach (var item in Books)
                        {
                            var pId = DbClass.entities.PublishingHouses.Where(x => x.Name == item.Title).Select(x => x.Id).First();
                            var aId = DbClass.entities.Authors.Where(x => x.FullName== item.AuthorFullName).Select(x => x.Id).First();
                            var book = new Book()
                            {
                                Quantity = item.Quantity,
                                Title = item.Title,
                                PublishingHouseId = pId,
                                AuthorId = aId
                            };
                            DbClass.entities.Books.AddOrUpdate(book);
                        }

                        DbClass.entities.SaveChanges();
                        MessageBox.Show("Изменения успешно сохранены");
                    }
                    catch (Exception ex)
                    {
                        string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                        FileStream fileStream = new FileStream(fileName, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fileStream);
                        sw.Write(ex.Message);
                        sw.Close();
                    }
                });
            }
        }

        private RelayCommand _backUsersCommand;
        public RelayCommand BackUsersCommand
        {
            get
            {
                return _backUsersCommand ??= new RelayCommand(obj =>
                {
                    Pagination.BackT(ref _books, listBooks);
                });
            }
        }

        private RelayCommand _forwardUsersCommand;
        public RelayCommand ForwardUsersCommand
        {
            get
            {
                return _forwardUsersCommand ??= new RelayCommand(obj =>
                {
                    Pagination.ForwardT(ref _books, listBooks);
                });
            }
        }

        private RelayCommand _lastUsersCommand;
        public RelayCommand LastUsersCommand
        {
            get
            {
                return _lastUsersCommand ??= new RelayCommand(obj =>
                {
                    Pagination.LastT(ref _books, listBooks);
                });
            }
        }
    }
}
