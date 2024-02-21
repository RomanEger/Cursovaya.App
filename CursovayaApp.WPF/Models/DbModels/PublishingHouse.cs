namespace CursovayaApp.WPF.Models.DbModels
{
    public class PublishingHouse : TableBase
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
