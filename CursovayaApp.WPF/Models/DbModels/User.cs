using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class User : TableBase
    {
        public override int Id { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role? Role { get; set; }
        public int RoleId { get; set; }
        public ICollection<RentalBook> RentalBooks { get; set; } = new List<RentalBook>();
    }
}
