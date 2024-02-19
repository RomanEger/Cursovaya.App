using CursovayaApp.WPF.Models.DbModels;

namespace CursovayaApp.WPF.Repository.Contracts;

public interface IAuthorRepository
{
    IEnumerable<Author> GetAll();
    Author Get(int id);
    int Add(Author newUser);
    int Update(Author user);
    int Delete(Author user);
}