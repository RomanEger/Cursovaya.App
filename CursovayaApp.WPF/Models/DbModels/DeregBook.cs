using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class DeregBook : TableBase
    {
        public override int Id { get; set; }
        public int BookId { get; set; }
        public int ReasonId { get; set; }
        public DateTime DateOfDereg { get; set; }
        public int DeregQuantity { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public Book? Book { get; set; }
        public ReasonDereg? Reason { get; set; }
    }
}
