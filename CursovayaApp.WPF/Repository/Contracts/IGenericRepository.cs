using CursovayaApp.WPF.Models.DbModels;

namespace CursovayaApp.WPF.Repository.Contracts;

public interface IGenericRepository<T> :IRepository where T: TableBase
{
    IEnumerable<T> GetAll();
    T Get(Func<T, bool> predicate);
    T Get(int id);
    int Add(T item);
    int AddOrUpdate(T item);

    int Update(T item);
    int Delete(T item);
    bool Any(Func<T, bool> predicate);
    IQueryable<T> Where(Func<T, bool> predicate);
    int Count(Func<T, bool> predicate);
}