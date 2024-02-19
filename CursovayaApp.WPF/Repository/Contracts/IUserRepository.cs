using CursovayaApp.WPF.Models.DbModels;

namespace CursovayaApp.WPF.Repository.Contracts;

public interface IUserRepository : IRepository
{
    IEnumerable<User> GetAll();
    User Get(int id);
    User Get(string login, string password);
    int Add(User newUser);
    int AddOrUpdate(User user);
    int Delete(User user);
    bool Any(int id);
}