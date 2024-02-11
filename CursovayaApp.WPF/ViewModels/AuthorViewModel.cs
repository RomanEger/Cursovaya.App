using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using System.Windows;

namespace CursovayaApp.WPF.ViewModels
{
    internal class AuthorViewModel : ViewModelBase
    {
        private Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                OnPropertyChanged();
            }
        }

        private Author Author { get; }

        private readonly bool _ForAdd;

        private readonly BooksViewModel _vm;

        public AuthorViewModel(Author author, BooksViewModel vm, bool ForAdd)
        {
            SelectedAuthor = author;
            Author = new Author()
            {
                Id = author.Id,
                BirthYear = author.BirthYear,
                DeathYear = author.DeathYear,
                FullName = author.FullName,
                Books = author.Books
            };
            _vm = vm;
            _ForAdd = ForAdd;
        }

        public RelayCommand AddOrUpdateAuthorCommand =>
            new (obj =>
            {
                if (_ForAdd)
                {
                    DbClass.entities.Authors.Add(SelectedAuthor);
                    _vm.Authors.Add(SelectedAuthor.FullName);
                    _vm.AuthorsForAdd.Add(SelectedAuthor.FullName);
                }
                DbClass.entities.SaveChanges();
            });

        public RelayCommand CancelCommand =>
            new (obj =>
            {
                SelectedAuthor = Author;
            });

        private bool _isChecked;

        public RelayCommand DeathYearCommand =>
            new (obj =>
            {
                if (!_isChecked)
                {
                    SelectedAuthor.DeathYear = null;
                    _isChecked = true;
                }
                else
                {
                    _isChecked = false;
                }
            });
    }
}
