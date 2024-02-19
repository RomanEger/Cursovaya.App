using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using System.Collections.ObjectModel;
using CursovayaApp.WPF.Views.Windows;

namespace CursovayaApp.WPF.ViewModels
{
    public partial class BooksViewModel
    {
        private AddOrUpdateBooks _addOrUpdateBooksView;

        private AddOrUpdateAuthors _addOrUpdateAuthorsView;

        private AddOrUpdatePublishings _addOrUpdatePublishingsView;

        private WindowForLibr _windowForGiveView;

        //private WindowForRecieve _windowForRecieveView;

        public ObservableCollection<string> ListReasons { get; set; }

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

        public ObservableCollection<string> ListPublishings { get; set; }

        private string _selectedPublishing;
        public string SelectedPublishing
        {
            get => _selectedPublishing;
            set
            {
                _selectedPublishing = value;
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
                OnPropertyChanged("SearchText");
            }
        }

        private List<BookView> _listBooks;

        private List<BookView> _sortedListBooks;

        public ObservableCollection<string> Authors { get; set; }

        public ObservableCollection<string> AuthorsForAdd { get; set; }

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
                GetPublishings();
                OnPropertyChanged("SelectedBook");
            }
        }

        private LoggedUser _loggedUser;
    }
}
