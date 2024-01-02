using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class RegBook
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int ReasonId { get; set; }
        public DateTime DateOfReg { get; set; }
        public int RegQuantity { get; set; }
        public Book? Book { get; set; }
        public ReasonReg? Reason { get; set; }
    }
}
