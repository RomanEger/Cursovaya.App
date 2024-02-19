using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.Repository;

public class RoleRepository : IRoleRepository
{
    private readonly DbContext _dbContext;

    public RoleRepository(DbContext dbDbContext)
    {
        _dbContext = dbDbContext;
    }

    public int Save() =>
        _dbContext.SaveChanges();

    public IEnumerable<Role> GetAll() =>
        _dbContext.Set<Role>().AsEnumerable();


    public Role Get(int id) =>
        _dbContext.Set<Role>().FirstOrDefault(x => x.Id == id) ?? new Role();

    public int Add(Role newRole)
    {
        _dbContext.Add(newRole);
        return _dbContext.SaveChanges();
    }

    public int AddOrUpdate(Role role)
    {
        _dbContext.Set<Role>().AddOrUpdate(role);
        return _dbContext.SaveChanges();
    }

    public int Delete(Role role)
    {
        _dbContext.Remove(role);
        return _dbContext.SaveChanges();
    }

    public bool Any(int id) =>
        _dbContext.Set<Role>().Any(x => x.Id == id);

}