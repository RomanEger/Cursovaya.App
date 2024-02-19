using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.Models.DbModels
{
    [NotMapped]
    public class BookView : TableBase, INotifyPropertyChanged
    {
        public BookView(int quantity)
        {
            OldQuantity = quantity;
        }

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

        private string _authorFullName;

        public string AuthorFullName
        {
            get => _authorFullName;
            set
            {
                _authorFullName = value;
                OnPropertyChanged();
            }
        }

        private int _oldQuantity;
        public int OldQuantity
        {
            get => _oldQuantity;
            init => _oldQuantity = value;
        }

        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity > value)
                {
                    QuantityToUpdate = _oldQuantity - value;
                    ForAdd = false;
                }
                else
                {
                    QuantityToUpdate = value - _oldQuantity;
                    ForAdd = true;
                }
                _quantity = value;
                OnPropertyChanged();
            }
        }

        private string _publishing;

        public string Publishing
        {
            get=>_publishing;
            set
            {
                _publishing = value;
                OnPropertyChanged();
            }
        }

        private int _quantityToUpdate;

        public int QuantityToUpdate
        {
            get => _quantityToUpdate;
            set
            {
                _quantityToUpdate = value;
                OnPropertyChanged();
            }
        }

        private bool _forAdd = false;

        public bool ForAdd
        {
            get => _forAdd;
            set
            {
                _forAdd = value;
                OnPropertyChanged();
            }
        }

        public void GetReasons()
        {
            try
            {
                if (ForAdd)
                {
                    var l = DbClass.entities.ReasonsReg.Select(x => x.Name).ToList();
                    ListReasons = new ObservableCollection<string>(l);
                }
                else
                {
                    var l = DbClass.entities.ReasonsDereg.Select(x => x.Name).ToList();
                    ListReasons = new ObservableCollection<string>(l);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int AddBook()
        {
            using var db = new ApplicationContext();
            var transaction = db.Database.BeginTransaction();
            try
            {
                var pId = DbClass.entities.PublishingHouses.Where(p => p.Name == Publishing).Select(x => x.Id).First();
                if (pId <= 0)
                    throw new Exception();
                var aId = DbClass.entities.Authors.Where(p => p.FullName == AuthorFullName).Select(x => x.Id).First();
                if (aId <= 0)
                    throw new Exception();
                var book = new Book()
                {
                    AuthorId = aId,
                    PublishingHouseId = pId,
                    Quantity = this.Quantity,
                    Title = this.Title
                };
                DbClass.entities.Books.Add(book);
                DbClass.entities.RegBooks.Add(new RegBook()
                    { BookId = book.Id, DateOfReg = DateTime.Now, ReasonId = 1, RegQuantity = book.Quantity });
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
                var pId = DbClass.entities.PublishingHouses.Where(p => p.Name == Publishing).Select(x => x.Id).FirstAsync();
                if (pId.Result <= 0)
                    throw new Exception();
                var aId = DbClass.entities.Authors.Where(p => p.FullName == AuthorFullName).Select(x => x.Id).FirstAsync();
                if (aId.Result <= 0)
                    throw new Exception();
                var book = new Book()
                {
                    AuthorId = aId.Result,
                    PublishingHouseId = pId.Result,
                    Quantity = this.Quantity,
                    Title = this.Title
                };
                await DbClass.entities.Books.AddAsync(book);
                await DbClass.entities.RegBooks.AddAsync(new RegBook()
                    { BookId = book.Id, DateOfReg = DateTime.Now, ReasonId = 1, RegQuantity = book.Quantity });
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

        private IEnumerable<string> _listReasons = new List<string>();
        public IEnumerable<string> ListReasons
        {
            get => _listReasons;
            set
            {
                _listReasons = value;
                OnPropertyChanged();
            }
        }
    }
}
