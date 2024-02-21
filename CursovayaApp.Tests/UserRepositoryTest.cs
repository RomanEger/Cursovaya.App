using CursovayaApp.Tests.Fixtures;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository;
using CursovayaApp.WPF.Repository.Contracts;
using Moq;

namespace CursovayaApp.Tests
{
    public class UserRepositoryTest
    {
        private Mock<IGenericRepository<User>> _mockUserRepository;
        private IFixture<User> _fixture;
        private ApplicationContext _dbContext;
        
        public UserRepositoryTest()
        {
            _mockUserRepository = new Mock<IGenericRepository<User>>();
            _dbContext = new ApplicationContext();
            _fixture = new UserFixture();
        }

        [Fact]
        public void TestAdd()
        {
            
            var user = new User()
            {
                FullName = "hellos",
                Login = "login111",
                Password = "qwerty",
                RoleId = 1
            };

            var add = _fixture.Add(user);

            _mockUserRepository.Setup(x => x.Add(user)).Returns(add);

            var repo = _mockUserRepository.Object;

            Assert.Equal(1, repo.Add(user));
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

            var add = _fixture.Add(user);

            _mockUserRepository.Setup(x => x.Add(user)).Returns(add);

            var repo = _mockUserRepository.Object;

            Assert.Equal(0, repo.Add(user));
        }
        
        [Fact]
        public void TestBadAdd1()
        {

            var user = new User();

            var add = _fixture.Add(user);

            _mockUserRepository.Setup(x => x.Add(user)).Returns(add);

            var repo = _mockUserRepository.Object;

            Assert.Equal(0, repo.Add(user));
        }

        [Fact]
        public void TestGetAll_RandomData()
        {
            const int count = 4;

            var list = _fixture.GetRandomData(count);

            _mockUserRepository.Setup(x => x.GetAll()).Returns(list);

            var repo = _mockUserRepository.Object;

            var result = repo.GetAll();

            var c = result.Count();

            Assert.Equal(
                list.Count(),
                result.Count());
        }

        [Fact]
        public void TestGetAll_DbData()
        {
            int expectedCount = 6;

            var repo = new GenericRepository<User>(_dbContext);

            var result = repo.GetAll();

            var actualCount = result.Count();

            Assert.Equal(
                expectedCount,
                actualCount);
        }

        [Fact]
        public void TestGet_TestData()
        {
            int id = 1;

            var list = _fixture.GetTestData();
            _mockUserRepository.Setup(x =>
                x.Get(id))
                .Returns(
                    list.ToList()[0]);

            var repo = _mockUserRepository.Object;

            var user = repo.Get(id);

            Assert.Equal(id, user.Id);
        }

        [Fact]
        public void TestGet_DbData_ById()
        {
            int id = 1001;

            var repo = new GenericRepository<User>(_dbContext);

            var user = repo.Get(x => x.Id == id);

            Assert.Equal(id, user.Id);
        }

        [Fact]
        public void TestGet_DbData_ByLoginAndPassword()
        {
            const int expectedId = 4;

            const string login = "test";
            
            const string password = "test";

            var repo = new GenericRepository<User>(_dbContext);

            var user = repo.Get(x => x.Login == login && x.Password == password);

            Assert.Equal(expectedId, user.Id);
        }

        [Fact]
        public void TestBadGet_DbData_ByLoginAndPassword()
        {
            const int expectedId = 0;

            const string login = "login";

            const string password = "1234";

            var repo = new GenericRepository<User>(_dbContext);

            var user = repo.Get(x => x.Login == login && x.Password == password);

            Assert.Equal(expectedId, user.Id);
        }
    }
}