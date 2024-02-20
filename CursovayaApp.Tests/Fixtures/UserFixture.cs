using CursovayaApp.WPF.Models.DbModels;
using Bogus;

namespace CursovayaApp.Tests.Fixtures;

public class UserFixture : IFixture<User>
{
    public Faker<User> GenerationRules() =>
        new Faker<User>()
            .RuleFor(x => x.FullName, r => r.Random.Word())
            .RuleFor(x => x.Password, r => r.Internet.Password())
            .RuleFor(x => x.Login, r => r.Random.Word())
            .RuleFor(x => x.RoleId, r => r.Random.Int(1, 4));
    
    public IEnumerable<User> GetRandomData(int count) =>
        GenerationRules().Generate(count);


    public IEnumerable<User> GetTestData() =>
        new List<User>()
        {
            new User()
            {
                FullName = "hello",
                Login = "login",
                Password = "qwerty",
                RoleId = 2
            },
            new User()
            {
                FullName = "hello",
                Login = "admin",
                Password = "qwerty",
                RoleId = 1
            }
        };
}