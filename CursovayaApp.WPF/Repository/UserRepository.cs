using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public User Get(string login, string password) =>
        _dbContext.Set<User>().FirstOrDefault(x => x.Login == login && x.Password == password) ?? new User();

}