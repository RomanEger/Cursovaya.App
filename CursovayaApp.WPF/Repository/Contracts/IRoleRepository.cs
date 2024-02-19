using CursovayaApp.WPF.Models.DbModels;

namespace CursovayaApp.WPF.Repository.Contracts;

public interface IRoleRepository : IRepository
{
    IEnumerable<Role> GetAll();
    Role Get(int id);
    int Add(Role newRole);
    int AddOrUpdate(Role role);
    int Delete(Role role);
    bool Any(int id);
}