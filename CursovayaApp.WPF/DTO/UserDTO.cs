using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.ViewModels;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CursovayaApp.WPF.DTO
{
    public class UserDTO : TableBase, INotifyPropertyChanged
    {
        public UserDTO() { }

        public UserDTO(AdminViewModel viewModel) 
        {
            _adminViewModel = new Lazy<AdminViewModel>(viewModel);
        }

        Lazy<AdminViewModel> _adminViewModel;
        private string _fullName = string.Empty;

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

        private string _login = string.Empty;

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

        private string _password = string.Empty;

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

        private string _role = string.Empty;

        public string Role
        {
            get => _role;
            set
            {
                if (string.IsNullOrEmpty(value))
                    _role = value;
                else if (value.Length > 50 ||
                   !_allowedRoles.Any(x => x.Value == value))
                    return;
                _role = value;
                if(_adminViewModel != null)
                {
                    _adminViewModel.Value.SelectedUser.RoleId = AllowedRoles.FirstOrDefault(x => x.Value == value).Key;
                    _adminViewModel.Value.SelectedUser.OnPropertyChanged();
                }
                
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public Dictionary<int, string> AllowedRoles => _allowedRoles;

        private readonly Dictionary<int, string> _allowedRoles = new Dictionary<int, string>()
        {
            {1, "Администратор" },
            {2, "Библиотекарь" },
            {3, "Кладовщик" },
            {4, "Клиент" }
        };
    }
}