using CursovayaApp.WPF.Models.DbModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CursovayaApp.WPF.ViewModels;

namespace CursovayaApp.WPF.Models
{
    public class LoggedUser : ViewModelBase
    {
        private static User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged("CurrentUser");
            } 
        }
    }
}
