using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class Author : TableBase, INotifyPropertyChanged
    {
        private int _id;
        public override int Id
        {
            get => _id; 
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

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

        private int _birthYear;
        public int BirthYear 
        { 
            get => _birthYear;
            set
            {
                _birthYear = value;
                OnPropertyChanged();
            }
        }

        private int? _deathYear;
        public int? DeathYear
        {
            get => _deathYear;
            set
            {
                _deathYear = value;
                OnPropertyChanged();
            }
        }
        public ICollection<Book> Books { get; set; } = new List<Book>();

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
