using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.ViewModels
{
    class AuthorViewModel : ViewModelBase
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
        private BooksViewModel _vm;
        public AuthorViewModel(Author author, BooksViewModel vm)
        {
            SelectedAuthor = author;
            _vm = vm;
        }

        private RelayCommand _addOrUpdateAuthorCommand;
        public RelayCommand AddOrUpdateAuthorCommand
        {
            get
            {
                return _addOrUpdateAuthorCommand ??= new RelayCommand(obj =>
                {
                    DbClass.entities.Authors.Add(SelectedAuthor);
                    _vm.Authors.Add(SelectedAuthor.FullName);
                    _vm.AuthorsForAdd.Add(SelectedAuthor.FullName);
                    DbClass.entities.SaveChanges();
                });
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ??= new RelayCommand(obj =>
                {

                });
            }
        }

        bool isCheched;
        private RelayCommand _deathYearCommand;
        public RelayCommand DeathYearCommand
        {
            get
            {
                return _deathYearCommand ??= new RelayCommand(obj =>
                {
                    if (!isCheched)
                    {
                        SelectedAuthor.DeathYear = null;
                        isCheched = true;
                    }
                    else
                        isCheched = false;

                });
            }
        }
    }
}
