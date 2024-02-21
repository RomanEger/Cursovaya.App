using TableBase = CursovayaApp.WPF.Models.DbModels.TableBase;

namespace CursovayaApp.Tests.Fixtures;

public interface IFixture<T> where T : TableBase
{
    IEnumerable<T> GetRandomData(int count);

    IEnumerable<T> GetTestData();

    int Add(T item);
}