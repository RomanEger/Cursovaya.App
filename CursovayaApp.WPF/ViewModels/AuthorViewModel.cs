using System.Collections.ObjectModel;
using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models.DbModels;
using System.Windows;
using CursovayaApp.WPF.Repository;
using CursovayaApp.WPF.Repository.Contracts;

namespace CursovayaApp.WPF.ViewModels
{
    internal class AuthorViewModel : ViewModelBase
    {
        private readonly IGenericRepository<Author> _repository;

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

        private readonly bool _forAdd = false;

        private readonly BooksViewModel _vm;

        public AuthorViewModel(Author author, BooksViewModel vm)
        {
            _repository = new GenericRepository<Author>(new ApplicationContext());
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
            if (author.Id < 1)
                _forAdd = true;
        }

        public RelayCommand AddOrUpdateAuthorCommand =>
            new (_ =>
            {
                try
                {
                    if (_forAdd)
                    {
                        _repository.Add(SelectedAuthor);
                        if (_vm.AuthorsForAdd != null)
                            _vm.AuthorsForAdd.Add(SelectedAuthor.FullName);
                        else
                            _vm.AuthorsForAdd = new ObservableCollection<string>(new List<string>() {SelectedAuthor.FullName});
                    }
                    _repository.Save();
                    MessageBox.Show("Автор успешно добавлен!");
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, "Не удалось сохранить изменения", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

        public RelayCommand CancelCommand =>
            new (_ =>
            {
                SelectedAuthor = _author;
            });

        private bool _isChecked;

        public RelayCommand DeathYearCommand =>
            new (_ =>
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
