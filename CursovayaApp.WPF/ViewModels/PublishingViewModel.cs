using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository;
using System.Windows;
using CursovayaApp.WPF.Repository.Contracts;

namespace CursovayaApp.WPF.ViewModels
{
    public class PublishingViewModel : ViewModelBase
    {
        private readonly IGenericRepository<PublishingHouse> _repository;

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
        private readonly Lazy<string> _name;
        private readonly BooksViewModel _vm;

        public PublishingViewModel(PublishingHouse publishingHouse, BooksViewModel booksViewModel)
        {
            _repository = new GenericRepository<PublishingHouse>(new ApplicationContext());
            Publishing = publishingHouse;
            _vm = booksViewModel;
            if (publishingHouse.Id < 1)
                ForAdd = true;
            else
                _name = new Lazy<string>(Publishing.Name);
        }

        public RelayCommand SaveCommand =>
            new (_ =>
            {
                if (ForAdd)
                {
                    try
                    {
                        if (_repository.Any(x => x.Name == Publishing.Name))
                        {
                            MessageBox.Show("Такое издательство уже существует!");
                            return;
                        }

                        _repository.Add(Publishing);
                        _vm.ListPublishings.Add(Publishing.Name);
                        _repository.Save();
                        MessageBox.Show("Сохранения успешно применены!");
                    }
                    catch (Exception)
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
                        _repository.Save();
                        _vm.GetPublishings();
                        MessageBox.Show("Сохранения успешно применены!");
                    }
                    catch (Exception)
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
