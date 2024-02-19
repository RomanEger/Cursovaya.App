using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository.Contracts;
using Moq;

namespace CursovayaApp.Tests
{
    public class UserRepositoryTest
    {
        private Mock<IUserRepository> _mockUserRepository;

        public UserRepositoryTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [Fact]
        public void TestBadAdd()
        {
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
    }
}