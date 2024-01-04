using CursovayaApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        private List<User> listUsers;
        private readonly int UsersAtPage = 5;
        private int IndexUser = 0;
        private int _count;

        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }
        public AdminViewModel()
        {
            GetUsers();
        }
        private void GetUsers()
        {
            listUsers =  DbClass.entities.Users.ToList();
            SetCount();
            InsertToUsers();
        }

        private void SetCount()
        {
            Count = (int)Math.Ceiling(listUsers.Count * 1.0 / UsersAtPage);
        }
        private void InsertToUsers()
        {
            if (listUsers.Count <= UsersAtPage)
            {
                Users = new ObservableCollection<User>(listUsers);
                return;
            }


            var i = listUsers.Count - IndexUser;
            if (i <= 0)
            {
                if (UsersAtPage <= IndexUser)
                    IndexUser -= UsersAtPage;
                else
                    IndexUser = 0; 
                i = UsersAtPage;
            }
            if (UsersAtPage > i)
                Users = new ObservableCollection<User>(listUsers.GetRange(IndexUser, i));
            else
                Users = new ObservableCollection<User>(listUsers.GetRange(IndexUser, UsersAtPage));
        }

        private User _selectedUser;

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged("Users");
            }
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        private RelayCommand _firstUsersCommand;

        public RelayCommand FirstUsersCommand
        {
            get
            {
                return _firstUsersCommand ??= new RelayCommand(obj =>
                {
                    IndexUser = 0;
                    InsertToUsers();
                });
            }
        }

        private RelayCommand _backUsersCommand;
        public RelayCommand BackUsersCommand
        {
            get
            {
                return _backUsersCommand ??= new RelayCommand(obj =>
                {
                    if (IndexUser < 5)
                        IndexUser = 0;
                    else
                        IndexUser -= UsersAtPage;
                    InsertToUsers();
                });
            }
        }

        private RelayCommand _forwardUsersCommand;
        public RelayCommand ForwardUsersCommand
        {
            get
            {
                return _forwardUsersCommand ??= new RelayCommand(obj =>
                {
                    var i = listUsers.Count - IndexUser;
                    var canGoForward = i >= 5;
                    if (canGoForward)
                        IndexUser += UsersAtPage;
                    InsertToUsers();
                });
            }
        }

        private RelayCommand _lastUsersCommand;
        public RelayCommand LastUsersCommand
        {
            get
            {
                return _lastUsersCommand ??= new RelayCommand(obj =>
                {
                    IndexUser = listUsers.Count - UsersAtPage;
                    if(IndexUser <= 0)
                        IndexUser = 0;
                    else if (IndexUser < 5)
                        IndexUser = listUsers.Count - IndexUser;
                    InsertToUsers();
                });
            }
        }


        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??= new RelayCommand(obj =>
                {
                    var a = DbClass.entities.Users.ToList();
                    foreach (var item in Users)
                    {
                        DbClass.entities.Users.AddOrUpdate(item);
                    }

                    DbClass.entities.SaveChanges();
                    MessageBox.Show("Изменения успешно сохранены");
                });
            }
        }

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??= new RelayCommand(obj =>
                {
                    var newUser = new User();
                    listUsers.Add(newUser);
                    var i = listUsers.Count - IndexUser;
                    var canGoForward = i > UsersAtPage;
                    if (canGoForward)
                        IndexUser += UsersAtPage;
                    InsertToUsers();
                    SelectedUser = newUser;
                    SetCount();
                });
            }
        }
        
        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand(obj =>
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
                        if(DbClass.entities.Users.Any(x => x.Id == SelectedUser.Id))
                            DbClass.entities.Users.Remove(SelectedUser);
                        var s = listUsers.IndexOf(SelectedUser);
                        listUsers.Remove(SelectedUser);
                        if (s >= listUsers.Count && s > 0)
                            SelectedUser = listUsers[--s];
                        else
                            SelectedUser = listUsers[s];
                        InsertToUsers();
                        SetCount();
                        MessageBox.Show("Пользователь удален");
                    }
                });
            }
        }

        private RelayCommand _changeCommand;
        public RelayCommand ChangeCommand
        {
            get
            {
                return _changeCommand ??= new RelayCommand(obj =>
                {
                    MyFrame.frame.Navigate(new BooksPage());
                });
            }
        }

        private RelayCommand _exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return _exitCommand ??= new RelayCommand(obj =>
                {
                    if (MessageBox.Show(
                            "Вы уверены, что хотите выйти из аккаунта?",
                            "Выход",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        var a = MessageBox.Show("Хотите ли вы сохранить изменения?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (a == MessageBoxResult.Yes)
                            DbClass.entities.SaveChanges();
                        else if(a == MessageBoxResult.Cancel)
                            return;
                        MyFrame.Navigate(new LoginPage());
                        MyFrame.ClearHistory();
                    }
                });
            }
        }
    }
}
