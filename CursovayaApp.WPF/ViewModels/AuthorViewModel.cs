using System.Collections.ObjectModel;
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

        private readonly Author Author;

        private readonly bool _ForAdd = false;

        private readonly BooksViewModel _vm;

        public AuthorViewModel(Author author, BooksViewModel vm)
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
            if (author.Id < 1)
                _ForAdd = true;
        }

        public RelayCommand AddOrUpdateAuthorCommand =>
            new (obj =>
            {
                try
                {
                    if (_ForAdd)
                    {
                        DbClass.entities.Authors.Add(SelectedAuthor);
                        _vm.Authors.Add(SelectedAuthor.FullName);
                        if (_vm.AuthorsForAdd != null)
                            _vm.AuthorsForAdd.Add(SelectedAuthor.FullName);
                        else
                            _vm.AuthorsForAdd = new ObservableCollection<string>(new List<string>() {SelectedAuthor.FullName});
                    }
                    DbClass.entities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Не удалось сохранить изменения");
                }
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
