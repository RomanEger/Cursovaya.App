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
        private bool IsUsersPage = true;

        public AdminViewModel()
        {
            BtnChangeContent = "История изменения книг";
            GetUsers();
        }

        private async void GetUsers()
        {
            var list = DbClass.entities.Users.ToList();
            Users = new ObservableCollection<User>(list);
        }

        private User _selectedUser;

        public ObservableCollection<User> Users
        {
            get;
            set;
        }

        private ObservableCollection<User> NewUsers
        {
            get; 
            set; 
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

        private string _btnChangeContent;

        public string BtnChangeContent
        {
            get => _btnChangeContent;
            set
            {
                _btnChangeContent = value;
                OnPropertyChanged("BtnChangeContent");
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
                    Users.Add(newUser);
                    SelectedUser = newUser;
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
                    if (MessageBox.Show(
                            "Вы уверены, что хотите удалить пользователя?",
                            "Удаление",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        if(DbClass.entities.Users.Any(x => x.Id == SelectedUser.Id))
                            DbClass.entities.Users.Remove(SelectedUser);
                        Users.Remove(SelectedUser);
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
                    if (IsUsersPage)
                    {
                        BtnChangeContent = "Редактировать пользователей";
                        IsUsersPage = false;
                    }
                    else
                    {
                        BtnChangeContent = "История изменения книг";
                        IsUsersPage = true;
                    }
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
