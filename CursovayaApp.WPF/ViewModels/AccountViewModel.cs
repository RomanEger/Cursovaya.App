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
using CursovayaApp.WPF.Commands;
using CursovayaApp.WPF.Models;

namespace CursovayaApp.WPF.ViewModels
{
    internal class AccountViewModel : INotifyPropertyChanged
    {
        private string _login;

        private string _password;
        
        //public ObservableCollection<Account> Accounts { get; set; }

        private RelayCommand _loginCommand;

        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ??
                       (_loginCommand = new RelayCommand(obj =>
                           {
                               var q = DbClass.entities.Users.Where(x => x.Login == Login && x.Password == Password);
                               if (q.Any())
                                   MessageBox.Show("hello");
                               else
                                   MessageBox.Show("bye");
                           })

                       );
            }

        }

        //public AccountViewModel()
        //{
        //    Accounts = new()
        //    {
        //        new() { Login="Hello", Password = "World" }
        //    };
        //}

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
