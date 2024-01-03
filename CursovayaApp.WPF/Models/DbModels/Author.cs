using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class Author : TableBase
    {
        public override int Id { get; set; }
        public string FullName { get; set; }
        public int BirthYear { get; set; }
        public int? DeathYear { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
