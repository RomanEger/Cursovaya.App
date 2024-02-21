using CursovayaApp.Tests.Fixtures;
using CursovayaApp.WPF.Models.DbModels;
using CursovayaApp.WPF.Repository;
using CursovayaApp.WPF.Repository.Contracts;
using Moq;

namespace CursovayaApp.Tests
{
    public class BookRepositoryTest
    {
        private Mock<IGenericRepository<Book>> _mockRepository;
        private IFixture<Book> _fixture;
        private ApplicationContext _dbContext;

        public BookRepositoryTest()
        {
            _mockRepository = new Mock<IGenericRepository<Book>>();
            _dbContext = new ApplicationContext();
            _fixture = new BookFixture();
        }
        [Fact]
        public void TestAdd()
        {

            var book = new Book()
            {
                Title = "hellos",
                AuthorId = 1,
                PublishingHouseId = 3,
                Quantity = 10                
            };

            var add = _fixture.Add(book);

            _mockRepository.Setup(x => x.Add(book)).Returns(add);

            var repo = _mockRepository.Object;

            Assert.Equal(1, repo.Add(book));
        }

        [Fact]
        public void TestBadAdd()
        {

            var book = new Book()
            {
                Title = "",
                AuthorId = 1,
                PublishingHouseId = 3,
                Quantity = 10
            };

            var add = _fixture.Add(book);

            _mockRepository.Setup(x => x.Add(book)).Returns(add);

            var repo = _mockRepository.Object;

            Assert.Equal(0, repo.Add(book));
        }

        [Fact]
        public void TestBadAdd1()
        {

            var book = new Book();

            var add = _fixture.Add(book);

            _mockRepository.Setup(x => x.Add(book)).Returns(add);

            var repo = _mockRepository.Object;

            Assert.Equal(0, repo.Add(book));
        }

        [Fact]
        public void TestGetAll_RandomData()
        {
            const int count = 4;

            var list = _fixture.GetRandomData(count);

            _mockRepository.Setup(x => x.GetAll()).Returns(list);

            var repo = _mockRepository.Object;

            var result = repo.GetAll();

            var c = result.Count();

            Assert.Equal(
                list.Count(),
                result.Count());
        }

        [Fact]
        public void TestGetAll_DbData()
        {
            int expectedCount = 3;

            var repo = new GenericRepository<Book>(_dbContext);

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
            _mockRepository.Setup(x =>
                x.Get(id))
                .Returns(
                    list.ToList()[0]);

            var repo = _mockRepository.Object;

            var user = repo.Get(id);

            Assert.Equal(id, user.Id);
        }

        [Fact]
        public void TestGet_DbData_ById()
        {
            const int id = 6;

            var repo = new GenericRepository<Book>(_dbContext);

            var book = repo.Get(x => x.Id == id) ?? new Book();

            Assert.Equal(id, book.Id);
        }

        [Fact]
        public void TestGet_DbData_ByTitleAndAuthorAndPublishing()
        {
            const int expectedId = 1002;

            const string title = "Анна Каренина";

            const int authorId = 1001;

            const int publishingId = 1;

            var repo = new GenericRepository<Book>(_dbContext);

            var book = repo.Get(x => x.Title == title && x.AuthorId == authorId && x.PublishingHouseId == publishingId) ?? new Book();

            Assert.Equal(expectedId, book.Id);
        }

        [Fact]
        public void TestBadGet_DbData_ByTitleAndAuthorAndPublishing()
        {
            const int expectedId = 0;

            const string title = "test";

            const int authorId = 10;

            const int publishingId = -6;

            var repo = new GenericRepository<Book>(_dbContext);

            var book = repo.Get(x => x.Title == title && x.AuthorId == authorId && x.PublishingHouseId == publishingId) ?? new Book();

            Assert.Equal(expectedId, book.Id);
        }
    }
}
