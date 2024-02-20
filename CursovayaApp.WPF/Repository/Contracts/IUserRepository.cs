using CursovayaApp.WPF.Models.DbModels;

namespace CursovayaApp.WPF.Repository.Contracts;

public interface IUserRepository : IGenericRepository<User>
{
    User Get(string login, string password);
}