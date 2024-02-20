using CursovayaApp.WPF.Models.DbModels;

namespace CursovayaApp.WPF.Repository.Contracts;

public interface IGenericRepository<T> :IRepository where T: TableBase
{
    IEnumerable<T> GetAll();
    T Get(int id);
    int Add(T item);
    int AddOrUpdate(T item);
    int Delete(T item);
    bool Any(int id);
    bool Any(Func<T, bool> predicate);
}