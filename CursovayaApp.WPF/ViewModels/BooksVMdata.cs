using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using System.Collections.ObjectModel;
using CursovayaApp.WPF.Repository.Contracts;
using CursovayaApp.WPF.Views.Windows;

namespace CursovayaApp.WPF.ViewModels
{
    public partial class BooksViewModel
    {
        private readonly IGenericRepository<Book> _repositoryBook;
        private readonly IGenericRepository<PublishingHouse> _repositoryPublishing;
        private readonly IGenericRepository<Author> _repositoryAuthor;
        private readonly IGenericRepository<RegBook> _repositoryRegBook;
        private readonly IGenericRepository<DeregBook> _repositoryDeregBook;
        private readonly IGenericRepository<ReasonReg> _repositoryReasonsReg;
        private readonly IGenericRepository<ReasonDereg> _repositoryReasonsDereg;


        private AddOrUpdateBooks _addOrUpdateBooksView;

        private AddOrUpdateAuthors _addOrUpdateAuthorsView;

        private AddOrUpdatePublishings _addOrUpdatePublishingsView;

        private WindowForLibr _windowForGiveView;

        public ObservableCollection<string> ListReasons
        {
            get => _listReasons;
            set
            {
                if (Equals(value, _listReasons)) return;
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
                OnPropertyChanged(nameof(AddOrUpdateBookCommand));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ListPublishings
        {
            get => _listPublishings;
            set
            {
                if (Equals(value, _listPublishings)) return;
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
                OnPropertyChanged(nameof(AddPublishingCommand));
                if(SelectedBook != null)
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
                OnPropertyChanged();
            }
        }

        private List<BookView> _listBooks;

        private List<BookView> _sortedListBooks;

        public ObservableCollection<string> Authors
        {
            get => _authors;
            private set
            {
                if (Equals(value, _authors)) return;
                _authors = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string>? AuthorsForAdd
        {
            get => _authorsForAdd;
            set
            {
                if (Equals(value, _authorsForAdd)) return;
                _authorsForAdd = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AddOrUpdateBookCommand));
            }
        }

        private string _selectedAuthor = string.Empty;
        public string SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged(nameof(AddAuthorCommand));
                Sort();
                SetCount();
                OnPropertyChanged();
            }
        }
        private string _selectedAuthorForAdd = string.Empty;
        public string SelectedAuthorForAdd
        {
            get => _selectedAuthorForAdd;
            set
            {
                _selectedAuthorForAdd = value;
                OnPropertyChanged(nameof(AddAuthorCommand));
                Sort();
                SetCount();
                OnPropertyChanged();
            }
        }

        private readonly PaginationService<BookView> _pagination;
        public PaginationService<BookView> Pagination
        {
            get => _pagination;
            init
            {
                _pagination = value;
                OnPropertyChanged();
            }
        }

        private ICollection<BookView> _books;
        public ICollection<BookView> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged(nameof(SaveCommand));
                OnPropertyChanged(nameof(AddCommand));
                OnPropertyChanged(nameof(AddOrUpdateBookCommand));
                OnPropertyChanged(nameof(CancelCommand));
                OnPropertyChanged();
            }
        }

        private BookView? _selectedBook;
        public BookView? SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(AddCommand));
                OnPropertyChanged(nameof(AddOrUpdateBookCommand));
                OnPropertyChanged(nameof(CancelCommand));
                GetPublishings();
                OnPropertyChanged();
            }
        }

        private BookView? _bookForUpdateInit;

        public BookView? BookForUpdate
        {
            get => _bookForUpdateInit;
            set
            {
                _bookForUpdateInit = value;
                OnPropertyChanged();
            }
        }
        
        private readonly LoggedUser _loggedUser;
        private ObservableCollection<string> _authors;
        private ObservableCollection<string>? _authorsForAdd;
        private ObservableCollection<string> _listReasons;
        private ObservableCollection<string> _listPublishings;
    }
}
