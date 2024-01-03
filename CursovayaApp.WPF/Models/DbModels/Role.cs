using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class Role : TableBase
    {
        public override int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
