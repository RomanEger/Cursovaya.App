using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CursovayaApp.WPF.ViewModels
{
    class PublishingViewModel : ViewModelBase
    {
        private bool ForAdd { get; }

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
            if(publishingHouse.Id < 1)
                ForAdd = true;
        }

        public RelayCommand SaveCommand =>
            new (obj =>
            {
                if (ForAdd)
                {
                    try
                    {
                        if (DbClass.entities.PublishingHouses.Any(x => x.Name == Publishing.Name))
                        {
                            MessageBox.Show("Такое издательство уже существует!");
                            return;
                        }

                        DbClass.entities.PublishingHouses.Add(Publishing);
                        DbClass.entities.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось добавить издательство. Попробуйте еще раз.",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        DbClass.entities.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Не удалось сохранить изменения. Попробуйте еще раз.",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            });

    }
}
