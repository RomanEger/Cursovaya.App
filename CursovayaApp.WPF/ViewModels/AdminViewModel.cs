using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using System.Windows;
using CursovayaApp.WPF.Repository;
using CursovayaApp.WPF.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        private readonly IGenericRepository<User> _userRepository;

        private List<User> _listUsers;

        private List<User> _sortedListUsers;

        private PaginationService<User> _pagination;

        public PaginationService<User> Pagination
        {
            get => _pagination;
            set
            {
                _pagination = value;
                OnPropertyChanged("Pagination");
            }
        }

        private List<string> _listRolesStr;

        public List<string> ListRolesStr
        {
            get => _listRolesStr;
            set
            {
                _listRolesStr = value;
                OnPropertyChanged();
            }
        }

        private List<Role> _listRoles;

        private string _selectedRole;

        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                if(value == "Все")
                {
                    _sortedListUsers = _listUsers;
                    SetCount();
                    Pagination.InsertToUsers(ref _users, _sortedListUsers);
                }
                else
                {
                    var v = _listRoles.Where(x => x.Name == value).Select(x => x.Id).FirstOrDefault();
                    _sortedListUsers = _listUsers.Where(x => x.RoleId == v).ToList();
                    SetCount();
                    Pagination.InsertToUsers(ref _users, _sortedListUsers);
                }
                OnPropertyChanged();
            }
        }

        public AdminViewModel()
        {
            _userRepository = new GenericRepository<User>(new ApplicationContext());
            IGenericRepository<Role> roleRepository = new GenericRepository<Role>(new ApplicationContext());
            Pagination = new PaginationService<User>(3);
            try
            {
                GetUsers();
                _listRoles = roleRepository.GetAll().ToList();
                ListRolesStr = new List<string>() { "Все" };
                ListRolesStr.AddRange(_listRoles.Select(x => x.Name).ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GetUsers()
        {
            try
            {
                _listUsers = _userRepository.GetAll().ToList();
                _sortedListUsers = _listUsers;
                SetCount();
                Pagination.InsertToUsers(ref _users, _sortedListUsers);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetCount() =>
            Pagination.Count = (int)Math.Ceiling(_sortedListUsers.Count * 1.0 / Pagination.TsAtPage);
        


        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        private ICollection<User> _users;
        public ICollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged("Users");
            }
        }

        public RelayCommand FirstUsersCommand =>
            new (obj => Pagination.FirstT(ref _users, _listUsers));

        public RelayCommand BackUsersCommand =>
             new (obj => Pagination.BackT(ref _users, _listUsers));

        public RelayCommand ForwardUsersCommand =>
            new (obj => Pagination.ForwardT(ref _users, _listUsers));

        public RelayCommand LastUsersCommand =>
            new (obj => Pagination.LastT(ref _users, _listUsers));

        public RelayCommand SaveCommand =>
            new ( obj =>
            {
                try
                {
                    foreach (var item in Users)
                    {
                        _userRepository.AddOrUpdate(item);
                    }
                    MessageBox.Show("Изменения успешно сохранены");
                    GetUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Не удалось сохранить изменения", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

        public RelayCommand AddCommand =>
            new(obj =>
            {
                User newUser = new User();
                _listUsers.Add(newUser);
                int i = _listUsers.Count - Pagination.IndexT;
                bool canGoForward = i > Pagination.TsAtPage;
                if (canGoForward)
                    Pagination.IndexT += Pagination.TsAtPage;

                Pagination.InsertToUsers(ref _users, _listUsers);
                SelectedUser = newUser;
                SetCount();
            });

        public RelayCommand DeleteCommand =>
            new (obj =>
            {
                if (SelectedUser == null)
                {
                    MessageBox.Show("Сначала выберите пользователя");
                    return;
                }
                if (MessageBox.Show(
                        "Вы уверены, что хотите удалить пользователя?",
                        "Удаление",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (_userRepository.Any(x => x.Id == SelectedUser.Id))
                        {
                            _userRepository.Delete(SelectedUser);
                        }

                        int s = _listUsers.IndexOf(SelectedUser);
                        _listUsers.Remove(SelectedUser);
                        if (s >= _listUsers.Count && s > 0)
                        {
                            SelectedUser = _listUsers[--s];
                        }
                        else
                        {
                            SelectedUser = _listUsers[s];
                        }

                        Pagination.InsertToUsers(ref _users, _listUsers);
                        SetCount();
                        MessageBox.Show("Пользователь удален");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });

        public RelayCommand ChangeCommand =>
            new (obj => MyFrame.frame.Navigate(new BooksPage()));

        public RelayCommand ExitCommand =>
            new (obj =>
            {
                if (MessageBox.Show(
                        "Вы уверены, что хотите выйти из аккаунта?",
                        "Выход",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    try
                    {
                        MessageBoxResult a = MessageBox.Show("Хотите ли вы сохранить изменения?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (a == MessageBoxResult.Yes)
                        {
                            _userRepository.Save();
                        }
                        else if (a == MessageBoxResult.Cancel)
                        {
                            return;
                        }

                        MyFrame.Navigate(new LoginPage());
                        MyFrame.ClearHistory();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
    }
}
