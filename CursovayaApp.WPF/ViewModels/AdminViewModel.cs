using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        private List<User> listUsers;

        private List<User> sortedListUsers;

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
                    sortedListUsers = listUsers;
                    SetCount();
                    Pagination.InsertToUsers(ref _users, sortedListUsers);
                }
                else
                {
                    var v = _listRoles.Where(x => x.Name == value).Select(x => x.Id).FirstOrDefault();
                    sortedListUsers = listUsers.Where(x => x.RoleId == v).ToList();
                    SetCount();
                    Pagination.InsertToUsers(ref _users, sortedListUsers);
                }
                OnPropertyChanged();
            }
        }

        public AdminViewModel()
        {
            Pagination = new PaginationService<User>(3);
            try
            {
                GetUsers();
                _listRoles = DbClass.entities.Roles.ToList();
                ListRolesStr = new List<string>() { "Все" };
                ListRolesStr.AddRange(_listRoles.Select(x => x.Name).ToList());
            }
            catch (Exception)
            {
                //string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                //FileStream fileStream = new FileStream(fileName, FileMode.Create);
                //StreamWriter sw = new StreamWriter(fileStream);
                //sw.Write(ex.Message);
                //sw.Close();
            }
        }
        private void GetUsers()
        {
            listUsers = DbClass.entities.Users.ToList();
            sortedListUsers = listUsers;
            SetCount();
            Pagination.InsertToUsers(ref _users, sortedListUsers);
        }

        private void SetCount() =>
            Pagination.Count = (int)Math.Ceiling(sortedListUsers.Count * 1.0 / Pagination.TsAtPage);
        


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
            new (obj => Pagination.FirstT(ref _users, listUsers));

        public RelayCommand BackUsersCommand =>
             new (obj => Pagination.BackT(ref _users, listUsers));

        public RelayCommand ForwardUsersCommand =>
            new (obj => Pagination.ForwardT(ref _users, listUsers));

        public RelayCommand LastUsersCommand =>
            new (obj => Pagination.LastT(ref _users, listUsers));

        public RelayCommand SaveCommand =>
            new ( obj =>
            {
                try
                {
                    foreach (User item in Users)
                    {
                        DbClass.entities.Users.AddOrUpdate(item);
                    }

                    DbClass.entities.SaveChanges();
                    MessageBox.Show("Изменения успешно сохранены");
                    GetUsers();
                }
                catch (Exception)
                {
                    //string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                    //FileStream fileStream = new FileStream(fileName, FileMode.Create);
                    //StreamWriter sw = new StreamWriter(fileStream);
                    //sw.Write(ex.Message);
                    //sw.Close();
                }
            });

        public RelayCommand AddCommand =>
            new(obj =>
            {
                User newUser = new User();
                listUsers.Add(newUser);
                int i = listUsers.Count - Pagination.IndexT;
                bool canGoForward = i > Pagination.TsAtPage;
                if (canGoForward)
                    Pagination.IndexT += Pagination.TsAtPage;

                Pagination.InsertToUsers(ref _users, listUsers);
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
                        if (DbClass.entities.Users.Any(x => x.Id == SelectedUser.Id))
                        {
                            DbClass.entities.Users.Remove(SelectedUser);
                        }

                        int s = listUsers.IndexOf(SelectedUser);
                        listUsers.Remove(SelectedUser);
                        if (s >= listUsers.Count && s > 0)
                        {
                            SelectedUser = listUsers[--s];
                        }
                        else
                        {
                            SelectedUser = listUsers[s];
                        }

                        Pagination.InsertToUsers(ref _users, listUsers);
                        SetCount();
                        DbClass.entities.SaveChanges();
                        MessageBox.Show("Пользователь удален");
                    }
                    catch (Exception)
                    {
                        //string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                        //FileStream fileStream = new FileStream(fileName, FileMode.Create);
                        //StreamWriter sw = new StreamWriter(fileStream);
                        //sw.Write(ex.Message);
                        //sw.Close();
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
                            DbClass.entities.SaveChanges();
                        }
                        else if (a == MessageBoxResult.Cancel)
                        {
                            return;
                        }

                        MyFrame.Navigate(new LoginPage());
                        MyFrame.ClearHistory();
                    }
                    catch (Exception)
                    {
                        //string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                        //FileStream fileStream = new FileStream(fileName, FileMode.Create);
                        //StreamWriter sw = new StreamWriter(fileStream);
                        //sw.Write(ex.Message);
                        //sw.Close();
                    }
                }
            });
    }
}
