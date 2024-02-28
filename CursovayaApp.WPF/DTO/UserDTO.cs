using CursovayaApp.WPF.Models.DbModels;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CursovayaApp.WPF.DTO
{
    public class UserDTO : TableBase, INotifyPropertyChanged
    {
        private string _fullName = null!;

        public string FullName
        {
            get => _fullName;
            set
            {
                if (value == null ||
                    value.Length > 120)
                    return;
                _fullName = value;
                OnPropertyChanged();
            }
        }

        private string _login = null!;

        public string Login
        {
            get => _login;
            set
            {
                if (value == null ||
                    value.Length > 50)
                    return;
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password = null!;

        public string Password
        {
            get => _password;
            set
            {
                if (value == null ||
                    value.Length > 50)
                    return;
                _password = value;  
                OnPropertyChanged();
            }
        }

        private string _role = null!;

        public string Role
        {
            get => _role;
            set
            {
                if (value == null ||
                    value.Length > 50 ||
                   !_allowedRoles.Contains(value))
                    return;
                _role = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private string[] _allowedRoles = new string[] { "Администратор", "Библиотекарь", "Кладовщик", "Клиент" };
    }
}