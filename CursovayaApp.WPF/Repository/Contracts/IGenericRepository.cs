using CursovayaApp.WPF.Models.DbModels;

namespace CursovayaApp.WPF.Repository.Contracts;

public interface IGenericRepository<T> where T: TableBase
{
    IEnumerable<T> GetAll();
    T Get(int id);
    int Add(T newEntitie);
    int Update(T entitie);
    int Delete(T entitie);
}