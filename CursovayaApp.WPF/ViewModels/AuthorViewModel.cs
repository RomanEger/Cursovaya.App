using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;

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

        private readonly Author _author;
        private readonly bool _ForAdd;
        private readonly BooksViewModel _vm;
        public AuthorViewModel(Author author, BooksViewModel vm, bool ForAdd)
        {
            SelectedAuthor = author;
            _author = new Author()
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

        private RelayCommand _addOrUpdateAuthorCommand;
        public RelayCommand AddOrUpdateAuthorCommand =>
            _addOrUpdateAuthorCommand ??= new RelayCommand(obj =>
            {
                if (_ForAdd)
                {
                    DbClass.entities.Authors.Add(SelectedAuthor);
                    _vm.Authors.Add(SelectedAuthor.FullName);
                    _vm.AuthorsForAdd.Add(SelectedAuthor.FullName);
                }
                DbClass.entities.SaveChanges();
            });

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand =>
            _cancelCommand ??= new RelayCommand(obj =>
            {
                SelectedAuthor = _author;
            });

        private bool isCheched;
        private RelayCommand _deathYearCommand;
        public RelayCommand DeathYearCommand =>
            _deathYearCommand ??= new RelayCommand(obj =>
            {
                if (!isCheched)
                {
                    SelectedAuthor.DeathYear = null;
                    isCheched = true;
                }
                else
                {
                    isCheched = false;
                }
            });
    }
}
