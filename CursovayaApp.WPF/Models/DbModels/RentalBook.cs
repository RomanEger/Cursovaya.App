using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class RentalBook : TableBase
    {
        public override int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public User? User { get; set; }
        public Book? Book { get; set; }
    }
}
