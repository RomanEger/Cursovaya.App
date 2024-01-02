using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class ReasonReg
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RegBook> RegBooks { get; set; } = new List<RegBook>();
    }
}
