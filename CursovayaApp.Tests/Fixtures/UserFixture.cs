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
                Id=1,
                FullName = "hello",
                Login = "loginADM",
                Password = "qwerty",
                RoleId = 1
            },
            new User()
            {
                Id=2,
                FullName = "hello",
                Login = "login123",
                Password = "qwerty",
                RoleId = 2
            },
            new User()
            {
                Id=3,
                FullName = "hello",
                Login = "login",
                Password = "qwerty",
                RoleId = 3
            },
            new User()
            {
                Id=4,
                FullName = "helslo",
                Login = "client",
                Password = "qwerty",
                RoleId = 4
            }
        };

    public int Add(User newUser)
    {
        var list = GetTestData().ToList();

        var count = list.Count;

        if(newUser.Id > 0 &&
           !list.Any(x => x.Id == newUser.Id) &&
           newUser.FullName?.Length >= 4 &&
           newUser.Login?.Length >= 4 &&
           !list.Any(x => x.Login == newUser.Login) &&
           newUser.Password?.Length >= 4)
        {
            return 0;
        }

        list.Add(newUser);

        return list.Count - count;
    }
}