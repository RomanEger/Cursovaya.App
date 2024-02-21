namespace CursovayaApp.WPF.Models.DbModels
{
    public class Photo : TableBase
    {
        public override int Id { get; set; }
        public int BookId { get; set; }
        public byte[]? Image { get; set; }
        public Book? Book { get; set; }
    }
}
