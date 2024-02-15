using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows;

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
                ListPublishings = new ObservableCollection<string>(DbClass.entities.PublishingHouses.Select(x => x.Name).ToList());
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
        private async Task GetBooksAsync()
        {
            var l = await 
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
                 }).ToListAsync();
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
                AuthorsForAdd = new ObservableCollection<string>(DbClass.entities.Authors.Select(x => x.FullName).AsQueryable());
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
