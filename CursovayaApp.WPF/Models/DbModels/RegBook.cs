using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class RegBook : TableBase
    {
        public override int Id { get; set; }
        public int BookId { get; set; }
        public int ReasonId { get; set; }
        public DateTime DateOfReg { get; set; }
        public int RegQuantity { get; set; }
        public int UserId { get; set; }
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
    }
}
