using CursovayaApp.WPF.Models;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CursovayaApp.WPF.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T: TableBase
{
    protected readonly DbContext _dbContext;

    public GenericRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Save() =>
        _dbContext.SaveChanges();

    public IEnumerable<T> GetAll() =>
        _dbContext.Set<T>().AsEnumerable();


    public T Get(Func<T, bool> predicate) =>
        _dbContext.Set<T>().FirstOrDefault(predicate);

    public T Get(int id) =>
        _dbContext.Set<T>().FirstOrDefault(x => x.Id == id);

    public int Add(T newItem)
    {
        _dbContext.Set<T>().Add(newItem);
        return _dbContext.SaveChanges();
    }

    public int AddOrUpdate(T item)
    {
        _dbContext.Set<T>().AddOrUpdate(item);
        return _dbContext.SaveChanges();
    }

    public int Update(T item)
    {
        _dbContext.Set<T>().Update(item);
        return _dbContext.SaveChanges();
    }
    public int Delete(T item)
    {
        _dbContext.Set<T>().Remove(item);
        return _dbContext.SaveChanges();
    }

    public bool Any(Func<T, bool> predicate) =>
        _dbContext.Set<T>().Any(predicate);

    public IQueryable<T> Where(Func<T, bool> predicate) =>
        _dbContext.Set<T>().Where(predicate).AsQueryable();

    public int Count(Func<T, bool> predicate) =>
        _dbContext.Set<T>().Count(predicate);
}