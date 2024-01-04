using CursovayaApp.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        public AdminViewModel()
        {
            Pagination = new PaginationService<User>(7);
            try
            {
                GetUsers();
            }
            catch (Exception ex)
            {
                string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                FileStream fileStream = new FileStream(fileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fileStream);
                sw.Write(ex.Message);
                sw.Close();
            }
        }
        private void GetUsers()
        {
            listUsers =  DbClass.entities.Users.ToList();
            SetCount();
            Pagination.InsertToUsers(ref _users, listUsers);
        }

        private void SetCount()
        {
            Pagination.Count = (int)Math.Ceiling(listUsers.Count * 1.0 / Pagination.TsAtPage);
        }
        

        private User _selectedUser;

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
                    Pagination.FirstT(ref _users, listUsers);
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
                    Pagination.BackT(ref _users, listUsers);
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
                    Pagination.ForwardT(ref _users, listUsers);
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
                    Pagination.LastT(ref _users, listUsers);
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
                    try
                    {
                        var a = DbClass.entities.Users.ToList();
                        foreach (var item in Users)
                        {
                            DbClass.entities.Users.AddOrUpdate(item);
                        }

                        DbClass.entities.SaveChanges();
                        MessageBox.Show("Изменения успешно сохранены");
                    }
                    catch (Exception ex)
                    {
                        string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                        FileStream fileStream = new FileStream(fileName, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fileStream);
                        sw.Write(ex.Message);
                        sw.Close();
                    }
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
                    var i = listUsers.Count - Pagination.IndexT;
                    var canGoForward = i > Pagination.TsAtPage;
                    if (canGoForward)
                        Pagination.IndexT += Pagination.TsAtPage;
                    Pagination.InsertToUsers(ref _users, listUsers);
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
                        try
                        {
                            if (DbClass.entities.Users.Any(x => x.Id == SelectedUser.Id))
                                DbClass.entities.Users.Remove(SelectedUser);
                            var s = listUsers.IndexOf(SelectedUser);
                            listUsers.Remove(SelectedUser);
                            if (s >= listUsers.Count && s > 0)
                                SelectedUser = listUsers[--s];
                            else
                                SelectedUser = listUsers[s];
                            Pagination.InsertToUsers(ref _users, listUsers);
                            SetCount();
                            MessageBox.Show("Пользователь удален");
                        }
                        catch (Exception ex)
                        {
                            string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                            FileStream fileStream = new FileStream(fileName, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fileStream);
                            sw.Write(ex.Message);
                            sw.Close();
                        }
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
                        try
                        {
                            var a = MessageBox.Show("Хотите ли вы сохранить изменения?", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (a == MessageBoxResult.Yes)
                                DbClass.entities.SaveChanges();
                            else if(a == MessageBoxResult.Cancel)
                                return;
                            MyFrame.Navigate(new LoginPage());
                            MyFrame.ClearHistory();
                        }
                        catch (Exception ex)
                        {
                            string fileName = $@"C:\Users\error{DateTime.Now}.txt";
                            FileStream fileStream = new FileStream(fileName, FileMode.Create);
                            StreamWriter sw = new StreamWriter(fileStream);
                            sw.Write(ex.Message);
                            sw.Close();
                        }
                    }
                });
            }
        }
    }
}
