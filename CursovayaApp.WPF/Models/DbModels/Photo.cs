using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class Photo
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public byte[]? Image { get; set; }
        public Book? Book { get; set; }
    }
}
