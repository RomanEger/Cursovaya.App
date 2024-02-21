using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using System.Collections.ObjectModel;
using System.Windows;
using CursovayaApp.WPF.Repository;

namespace CursovayaApp.WPF.ViewModels
{
    public partial class BooksViewModel : ViewModelBase
    {
        private void Sort()
        {
            _sortedListBooks = _listBooks;
            if (!string.IsNullOrEmpty(SelectedAuthor) && SelectedAuthor != "Все")
                _sortedListBooks = _sortedListBooks.Where(x => x.AuthorFullName == SelectedAuthor).ToList();

            _sortedListBooks = _sortedListBooks.Where(x => x.Title.ToLower().Contains(SearchText.ToLower())).ToList();
            Pagination.InsertToUsers(ref _books, _sortedListBooks);
        }

        private void SetCount() =>
            Pagination.Count = (int)Math.Ceiling(_sortedListBooks.Count * 1.0 / Pagination.TsAtPage);

        private void GetData()
        {
            GetBooks();
            GetAuthors();
            SetCount();
        }

        public void GetPublishings()
        {
            try
            {
                ListPublishings = new ObservableCollection<string>(_repositoryPublishing.GetAll().Select(x => x.Name).ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }

        private void GetBooks()
        {
            try
            {
                var l =
                    (from book in _repositoryBook.GetAll()
                     join author in _repositoryAuthor.GetAll()
                         on book.AuthorId equals author.Id
                     join publishing in _repositoryPublishing.GetAll()
                         on book.PublishingHouseId equals publishing.Id
                     select new
                     {
                         book.Id,
                         book.Title,
                         AuthorFullName = author.FullName,
                         book.Quantity,
                         Publishing = publishing.Name
                     }).AsQueryable();
                _listBooks = new List<BookView>();
                foreach (var item in l)
                {
                    _listBooks.Add(new BookView(item.Quantity)
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void GetAuthors()
        {
            Authors = new ObservableCollection<string> { "Все" };
            var l = Books.Select(x => x.AuthorFullName).Distinct().AsQueryable();
            foreach (var item in l) 
                Authors.Add(item);   
            
        }

        public void InitAuthors()
        {
            try
            {
                AuthorsForAdd = new ObservableCollection<string>(_repositoryAuthor.GetAll().Select(x => x.FullName).AsQueryable());
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public BooksViewModel()
        {
            Pagination = new PaginationService<BookView>(7);
            
            _loggedUser = new();
            
            _repositoryBook = new GenericRepository<Book>(new ApplicationContext());
            _repositoryAuthor = new GenericRepository<Author>(new ApplicationContext());
            _repositoryRegBook = new GenericRepository<RegBook>(new ApplicationContext());
            _repositoryDeregBook = new GenericRepository<DeregBook>(new ApplicationContext());
            _repositoryPublishing = new GenericRepository<PublishingHouse>(new ApplicationContext());
            _repositoryReasonsReg = new GenericRepository<ReasonReg>(new ApplicationContext());
            _repositoryReasonsDereg = new GenericRepository<ReasonDereg>(new ApplicationContext());
            
            try
            {
                GetData();
            }
            catch
            {
                //
            }
        }
    }
}
