namespace CursovayaApp.WPF.Models.DbModels
{
    public class ReasonReg : TableBase
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RegBook> RegBooks { get; set; } = new List<RegBook>();
    }
}
