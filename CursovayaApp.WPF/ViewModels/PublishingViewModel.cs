using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.ViewModels
{
    class PublishingViewModel : ViewModelBase
    {
        private PublishingHouse _publishing;

        public PublishingHouse Publishing
        {
            get => _publishing;
            set
            {
                _publishing = value;
                OnPropertyChanged();
            }
        }

        private readonly BooksViewModel _vm;

        public PublishingViewModel(PublishingHouse publishingHouse, BooksViewModel booksViewModel)
        {
            Publishing = publishingHouse;
            _vm = booksViewModel;
        }

    }
}
