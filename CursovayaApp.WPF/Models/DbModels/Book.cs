using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class Book : TableBase
    {
        public override int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int PublishingId { get; set; }
        public int Quantity { get; set; }
        public Author? Author { get; set; }
        public PublishingHouse? PublishingHouse { get; set; }
        public ICollection<RegBook> RegBooks { get; set; } = new List<RegBook>();
        public ICollection<DeregBook> DeregBooks { get; set; } = new List<DeregBook>();
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
