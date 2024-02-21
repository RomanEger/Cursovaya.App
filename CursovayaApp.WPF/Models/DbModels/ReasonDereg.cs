namespace CursovayaApp.WPF.Models.DbModels
{
    public class ReasonDereg : TableBase
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public ICollection<DeregBook> DeregBooks { get; set; } = new List<DeregBook>();
    }
}
