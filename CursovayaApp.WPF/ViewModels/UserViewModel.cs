using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using System.Windows;
using CursovayaApp.WPF.Repository;
using CursovayaApp.WPF.Repository.Contracts;

namespace CursovayaApp.WPF.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private IGenericRepository<User> _repository;
        
        private User _thisUser;

        public User ThisUser
        {
            get => _thisUser;
            set
            {
                _thisUser = value;
                OnPropertyChanged();
            }
        }
        
        public RelayCommand LoginCommand =>
            new (_ =>
                {
                    try
                    {
                        var q = _repository.Get(x => x.Login == ThisUser.Login && x.Password == ThisUser.Password) ?? new User();
                        LoggedUser loggedUser = new()
                        {
                            CurrentUser = q
                        };
                        if (q.RoleId == 1)
                        {
                            MyFrame.Navigate(new AdminPage());
                        }
                        else if (q.RoleId is 2 or 3)
                        {
                            MyFrame.Navigate(new BooksPage());
                        }
                        else
                        {
                            MessageBox.Show("Неправильный логин или пароль");
                        }

                        ThisUser.Password = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public UserViewModel()
#pragma warning restore CS8618
        {
            ThisUser = new User();
            _repository = new GenericRepository<User>(new ApplicationContext());
        }
    }
}
