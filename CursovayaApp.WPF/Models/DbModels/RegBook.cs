using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class RegBook : TableBase, INotifyPropertyChanged
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

        [NotMapped] private int _bookId;

        public int BookId
        {
            get => _bookId;
            set
            {
                _bookId = value;
                OnPropertyChanged();
            }
        }

        [NotMapped] private int _reasonId;

        public int ReasonId
        {
            get => _reasonId;
            set
            {
                _reasonId = value;
                OnPropertyChanged();
            }
        }

        [NotMapped] private DateTime _dateOfReg;

        public DateTime DateOfReg
        {
            get => _dateOfReg;
            set
            {
                _dateOfReg = value;
                OnPropertyChanged();
            }
        }

        [NotMapped] private int _regQuantity;

        public int RegQuantity
        {
            get => _regQuantity;
            set
            {
                _regQuantity = value;
                OnPropertyChanged();
            }
        }

        [NotMapped] private int _userId;

        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
            }
        }

        public User? User { get; set; }
        public Book? Book { get; set; }
        public ReasonReg? Reason { get; set; }

        public int AddBook()
        {
            using var db = new ApplicationContext();
            var transaction = db.Database.BeginTransaction();
            try
            {
                var book = DbClass.entities.Books.FirstOrDefault(x => x.Id == BookId);
                if (book == null)
                    throw new Exception("Книга не найдена");

                book.Quantity += RegQuantity;
                DbClass.entities.Books.Update(book);
                DbClass.entities.RegBooks.Add(this);
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
            using var db = new ApplicationContext();
            var transaction = db.Database.BeginTransaction();
            try
            {
                var book = await DbClass.entities.Books.FirstOrDefaultAsync(x => x.Id == BookId);
                if (book == null)
                    throw new Exception("Книга не найдена");

                book.Quantity += RegQuantity;
                DbClass.entities.Update(book);
                await DbClass.entities.RegBooks.AddAsync(this);
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
