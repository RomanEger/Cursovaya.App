using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class User : TableBase, INotifyPropertyChanged
    {
        [NotMapped] private int _id;
        public override int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        private string _fullName;

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        private string _login;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        private int _roleId;

        public int RoleId 
        { 
            get => _roleId;
            set
            {
                _roleId = value;
                OnPropertyChanged();
            }
        }

        public Role? Role { get; set; }
        public ICollection<RentalBook> RentalBooks { get; set; } = new List<RentalBook>();


        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
