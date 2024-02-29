using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class Book : TableBase, INotifyPropertyChanged
    {
        [NotMapped]
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

        [NotMapped]
        private string _title;
        public string Title 
        { 
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        private int _authorId;
        public int AuthorId { get => _authorId; set
            {
                _authorId = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        private int _publishingHouseId;
        public int PublishingHouseId { get => _publishingHouseId; set
            {
                _publishingHouseId = value;
                OnPropertyChanged();
            }
        }

        [NotMapped]
        private int _quantity;
        public int Quantity { get => _quantity; set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        public Author? Author { get; set; }
        public PublishingHouse? PublishingHouse { get; set; }
        public ICollection<RegBook> RegBooks { get; set; } = new List<RegBook>();
        public ICollection<DeregBook> DeregBooks { get; set; } = new List<DeregBook>();


        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
