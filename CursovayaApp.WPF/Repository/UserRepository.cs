using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.Repository;

public class UserRepository : IUserRepository
{
    private readonly DbContext _dbContext;

    public UserRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Save() =>
        _dbContext.SaveChanges();

    public IEnumerable<User> GetAll() =>
        _dbContext.Set<User>().AsEnumerable();


    public User Get(int id) =>
        _dbContext.Set<User>().FirstOrDefault(x => x.Id == id) ?? new User();


    public User Get(string login, string password) =>
        _dbContext.Set<User>().FirstOrDefault(x => x.Login == login && x.Password == password) ?? new User();


    public int Add(User newUser)
    {
        _dbContext.Set<User>().Add(newUser);
        return _dbContext.SaveChanges();
    }

    public int AddOrUpdate(User user)
    {
        _dbContext.Set<User>().AddOrUpdate(user);
        return _dbContext.SaveChanges();
    }

    public int Delete(User user)
    {
        _dbContext.Set<User>().Remove(user);
        return _dbContext.SaveChanges();
    }

    public bool Any(int id) =>
        _dbContext.Set<User>().Any(x => x.Id == id);

}