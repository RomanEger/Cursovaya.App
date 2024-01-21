using System.Collections.ObjectModel;
using System.Windows;
using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.ViewModels
{
    public class BooksViewModel : ViewModelBase
    {
        private ObservableCollection<string> _listReasons;
        public ObservableCollection<string> ListReasons
        {
            get => _listReasons;
            set
            {
                _listReasons = value;
                OnPropertyChanged();
            }
        }

        private string _selectedReason;
        public string SelectedReason
        {
            get => _selectedReason;
            set
            {
                _selectedReason = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _listPublishings;
        public ObservableCollection<string> ListPublishings
        {
            get => _listPublishings;
            set
            {
                _listPublishings = value;
                OnPropertyChanged();
            }
        }

        private string _selectedPublishing;
        public string SelectedPublishing
        {
            get => _selectedPublishing;
            set
            {
                _selectedPublishing = value;
                SelectedBook.Publishing = _selectedPublishing;
                OnPropertyChanged();
            }
        }

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

        private List<BookView> _listBooks;

        private List<BookView> _sortedListBooks;

        public ObservableCollection<string> Authors { get; set; }

        private string _selectedAuthor = string.Empty;
        public string SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                Sort();
                SetCount();
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
                GetReasons();
                GetPublishings();
                OnPropertyChanged("SelectedBook");
            }
        }
        
        private void Sort()
        {
                _sortedListBooks = _listBooks;
                if (!string.IsNullOrEmpty(SelectedAuthor) && SelectedAuthor != "Все")
                    _sortedListBooks = _sortedListBooks.Where(x => x.AuthorFullName == SelectedAuthor).ToList();
                _sortedListBooks = _sortedListBooks.Where(x => x.Title.ToLower().Contains(SearchText.ToLower())).ToList();
                Pagination.InsertToUsers(ref _books, _sortedListBooks);
        }

        private void SetCount()
        {
            Pagination.Count = (int)Math.Ceiling(_sortedListBooks.Count * 1.0 / Pagination.TsAtPage);
        }

        private void GetData()
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
            _listBooks = new List<BookView>();
            foreach (var item in l)
            {
                _listBooks.Add(new BookView()
                {
                    Id = item.Id,
                    Title=item.Title,
                    AuthorFullName=item.AuthorFullName,
                    Quantity=item.Quantity,
                    Publishing=item.Publishing
                });
            }

            _sortedListBooks = _listBooks;
            Books = new ObservableCollection<BookView>();
            Pagination.InsertToUsers(ref _books, _listBooks);
        }

        private void GetPublishings()
        {
            var l = DbClass.entities.PublishingHouses.Select(x => x.Name).ToList();
            ListPublishings = new ObservableCollection<string>(l);
        }

        private void GetReasons()
        {
            if (SelectedBook.ForAdd)
            {
                var l = DbClass.entities.ReasonsReg.Select(x => x.Name).ToList();
                ListReasons = new ObservableCollection<string>(l);
            }
            else
            {
                var l = DbClass.entities.ReasonsDereg.Select(x => x.Name).ToList();
                ListReasons = new ObservableCollection<string>(l);
            }
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
            _listBooks = new List<BookView>();
            foreach (var item in l)
            {
                _listBooks.Add(new BookView()
                {
                    Id = item.Id,
                    Title = item.Title,
                    AuthorFullName = item.AuthorFullName,
                    Quantity = item.Quantity,
                    Publishing = item.Publishing
                });
            }

            _sortedListBooks = _listBooks;
            Books = new ObservableCollection<BookView>();
            Pagination.InsertToUsers(ref _books, _listBooks);
        }

        private void GetAuthors()
        {
            Authors = new ObservableCollection<string>(_listBooks.Select(x => x.AuthorFullName).Distinct());
            Authors.Insert(0, "Все");
        }

        private LoggedUser _loggedUser;
        public BooksViewModel()
        {
            Pagination = new PaginationService<BookView>(7);
            _loggedUser = new();
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
                    if (MyFrame.frame.CanGoBack)
                    {
                        
                        if (_loggedUser.CurrentUser.RoleId != 1)
                        {
                            if (MessageBox.Show(
                                    "Это действие приведет к выходу из аккаунта. Вы уверены, что хотите продолжить?",
                                    "Выход",
                                    MessageBoxButton.YesNo, 
                                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                MyFrame.frame.GoBack();
                            }
                        }
                        else
                        {
                            MyFrame.frame.GoBack();
                        }
                    }
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
                            var pId = DbClass.entities.PublishingHouses.Where(x => x.Name == item.Publishing).Select(x => x.Id).FirstOrDefault();
                            var aId = DbClass.entities.Authors.Where(x => x.FullName== item.AuthorFullName).Select(x => x.Id).FirstOrDefault();
                            var book = new Book()
                            {
                                Id = item.Id,
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
                        //string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                        //FileStream fileStream = new FileStream(fileName, FileMode.Create);
                        //StreamWriter sw = new StreamWriter(fileStream);
                        //sw.Write(ex.Message);
                        //sw.Close();
                    }
                });
            }
        }

        private AddOrUpdateBooks newWindow;
        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??= new RelayCommand(obj =>
                {
                    newWindow = new(this);
                    SelectedBook = new BookView();
                    Books.Add(SelectedBook);
                    newWindow.ShowDialog();
                });
            }
        }
        private RelayCommand _updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return _updateCommand ??= new RelayCommand(obj =>
                {
                    newWindow = new(this);
                    newWindow.ShowDialog();
                });
            }
        }

        private RelayCommand _firstBooksCommand;
        public RelayCommand FirstBooksCommand
        {
            get
            {
                return _firstBooksCommand ??= new RelayCommand(obj =>
                {
                    Pagination.FirstT(ref _books, _sortedListBooks);
                });
            }
        }

        private RelayCommand _backBooksCommand;
        public RelayCommand BackBooksCommand
        {
            get
            {
                return _backBooksCommand ??= new RelayCommand(obj =>
                {
                    Pagination.BackT(ref _books, _sortedListBooks);
                });
            }
        }

        private RelayCommand _forwardBooksCommand;
        public RelayCommand ForwardBooksCommand
        {
            get
            {
                return _forwardBooksCommand ??= new RelayCommand(obj =>
                {
                    Pagination.ForwardT(ref _books, _sortedListBooks);
                });
            }
        }

        private RelayCommand _lastBooksCommand;
        public RelayCommand LastBooksCommand
        {
            get
            {
                return _lastBooksCommand ??= new RelayCommand(obj =>
                {
                    Pagination.LastT(ref _books, _sortedListBooks);
                });
            }
        }

        private RelayCommand _addOrUpdateBookCommand;

        public RelayCommand AddOrUpdateBookCommand
        {
            get
            {
                return _addOrUpdateBookCommand ??= new RelayCommand(obj =>
                {
                    if (Books.Any(x => x.Id == SelectedBook.Id))
                    {
                        if (!Authors.Contains(SelectedBook.AuthorFullName))
                        {
                            if (MessageBox.Show(
                                    "Данный автор еще не представлен в нашей библиотеке. Хотите его добавть?",
                                    "",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                //открытие окна с добавлением автора
                            }
                            else return;
                        }

                        var aId = DbClass.entities.Authors.Where(x => x.FullName == SelectedAuthor).Select(x => x.Id).FirstOrDefault();
                        var pId = DbClass.entities.PublishingHouses.Where(x => x.Name == SelectedPublishing).Select(x => x.Id).FirstOrDefault();
                        var book = new Book()
                        {
                            Id = SelectedBook.Id,
                            Quantity = SelectedBook.Quantity,
                            Title = SelectedBook.Title,
                            AuthorId = aId,
                            PublishingHouseId = pId
                        };
                        DbClass.entities.Books.Add(book);
                        if (SelectedBook.ForAdd)
                        {
                            var rId = DbClass.entities.ReasonsReg.Where(x => x.Name == SelectedReason).Select(x => x.Id)
                                .FirstOrDefault();
                            var regBook = new RegBook()
                            {
                                BookId = book.Id,
                                ReasonId = rId,
                                DateOfReg = DateTime.Now,
                                UserId = _loggedUser.CurrentUser.Id,
                                RegQuantity = SelectedBook.QuantityToUpdate
                            };
                            DbClass.entities.RegBooks.Add(regBook);
                        }
                        else
                        {
                            var dId = DbClass.entities.ReasonsReg.Where(x => x.Name == SelectedReason).Select(x => x.Id)
                                .FirstOrDefault();
                            var deregBook = new DeregBook()
                            {
                                BookId = book.Id,
                                ReasonId = dId,
                                DateOfDereg = DateTime.Now,
                                UserId = _loggedUser.CurrentUser.Id,
                                DeregQuantity = SelectedBook.QuantityToUpdate
                            };
                            DbClass.entities.DeregBooks.Add(deregBook);
                            
                        }

                        newWindow.Close();
                    }
                });
            }
        }
    }
}
