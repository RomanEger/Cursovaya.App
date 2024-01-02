using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class ReasonDereg
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<DeregBook> DeregBooks { get; set; } = new List<DeregBook>();
    }
}
