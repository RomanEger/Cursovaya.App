using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
        public string Title { get => _title; set
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
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();


        public int AddBook()
        {
            using var db = new ApplicationContext();
            var transaction = db.Database.BeginTransaction();
            try
            {
                DbClass.entities.Books.Add(this);
                DbClass.entities.RegBooks.Add(new RegBook()
                { BookId = this.Id, DateOfReg = DateTime.Now, ReasonId = 1, RegQuantity = this.Quantity });
                transaction.Commit();
                return 0;
            }
            catch
            {
                transaction.Rollback();
                return -1;
            }
        }

        public async Task<int> AddBookAsync()
        {
            await using var db = new ApplicationContext();
            var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                await DbClass.entities.Books.AddAsync(this);
                await DbClass.entities.RegBooks.AddAsync(new RegBook()
                { BookId = this.Id, DateOfReg = DateTime.Now, ReasonId = 1, RegQuantity = this.Quantity });
                await transaction.CommitAsync();
                return 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                return -1;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
