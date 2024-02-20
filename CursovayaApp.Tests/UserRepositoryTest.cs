using CursovayaApp.Tests.Fixtures;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository;
using CursovayaApp.WPF.Repository.Contracts;
using Moq;

namespace CursovayaApp.Tests
{
    public class UserRepositoryTest
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ApplicationContext> _mockDbContext;
        private IFixture<User> _fixture;
        
        public UserRepositoryTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockDbContext = new Mock<ApplicationContext>();
            _fixture = new UserFixture();
        }

        [Fact]
        public void TestBadAdd()
        {
            var user = new User()
            {
                FullName = "",
                Login = "",
                Password = "",
                RoleId = 1
            };
            _mockUserRepository.Setup(x => x.Add(user)).Throws(new Exception());
            _mockUserRepository.Setup(x => x.Add(new User())).Throws(new Exception());
        }

        [Fact]
        public void TestAdd()
        {
            
            var user = new User()
            {
                FullName = "hello",
                Login = "login",
                Password = "qwerty",
                RoleId = 1
            };
            _mockUserRepository.Setup(x => x.Add(user)).Returns(1);
        }

        [Fact]
        public void TestGetAll()
        {
            const int count = 4;
            var list = _fixture.GetRandomData(count);

            _mockUserRepository.Setup(x => x.GetAll()).Returns(list);

            var repo = new UserRepository();

            var result = repo.GetAll();
            var c = result.Count();
            Assert.Equal(
                list.Count(),
                result.Count());
        }
    }
}