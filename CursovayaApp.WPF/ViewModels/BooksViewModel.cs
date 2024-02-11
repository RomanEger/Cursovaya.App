using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

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
        
        private async void GetData()
        {
            await GetBooksAsync();
            await GetAuthorsAsync();
            SetCount();
        }

        private void GetPublishings() =>
            ListPublishings = new ObservableCollection<string>(DbClass.entities.PublishingHouses.Select(x => x.Name).ToList());
        private async Task GetPublishingsAsync() =>
            ListPublishings = new ObservableCollection<string>(await DbClass.entities.PublishingHouses.Select(x => x.Name).ToListAsync());
        
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
            List<string> l = DbClass.entities.Authors.Select(x => x.FullName).ToList();
            Authors = new ObservableCollection<string>(l.Distinct());
            AuthorsForAdd = new ObservableCollection<string>(l.Distinct());
            Authors.Insert(0, "Все");
        }
        private async Task GetAuthorsAsync()
        {
            List<string> l = await DbClass.entities.Authors.Select(x => x.FullName).ToListAsync();
            Authors = new ObservableCollection<string>(l.Distinct());
            AuthorsForAdd = new ObservableCollection<string>(l.Distinct());
            Authors.Insert(0, "Все");
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
