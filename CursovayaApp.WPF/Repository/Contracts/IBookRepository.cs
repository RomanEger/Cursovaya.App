using CursovayaApp.WPF.Models.DbModels;

namespace CursovayaApp.WPF.Repository.Contracts;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book Get(int id);
    int Add(Book newUser);
    int Update(Book user);
    int Delete(Book user);
}