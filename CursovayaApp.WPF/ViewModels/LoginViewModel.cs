using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Services;
using CursovayaApp.WPF.Views;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private User _thisUser;

        public User ThisUser
        {
            get => _thisUser;
            set
            {
                _thisUser = value;
                OnPropertyChanged("User");
            }
        }


        private RelayCommand _loginCommand;

        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ??= new RelayCommand(obj =>
                    {
                               var q = DbClass.entities.Users.FirstOrDefault(x => x.Login == ThisUser.Login && x.Password == ThisUser.Password) ?? new User();
                               if (q.RoleId == 1)
                                   MyFrame.Navigate(new AdminPage());
                               else if (q.RoleId == 2)
                                   MyFrame.Navigate(new BooksPage());
                               else
                                   MessageBox.Show("Неправильный логин или пароль");
                               ThisUser.Password = null;
                    })
;
            }

        }

        public LoginViewModel()
        {
            ThisUser = new User();
        }
    }
}
