using CursovayaApp.WPF.DTO;

namespace CursovayaApp.Tests
{
    public class UserDTO_Test
    {
        [Fact]
        public void TestClient()
        {
            var user = new UserDTO()
            {
                FullName = "Test",
                Login = "test",
                Password = "test",
                Role = "Клиент"
            };
            Assert.Equal("Клиент", user.Role);
        }

        [Fact]
        public void TestAdmin()
        {
            var user = new UserDTO()
            {
                FullName = "Test",
                Login = "test",
                Password = "test",
                Role = "Администратор"
            };
            Assert.Equal("Администратор", user.Role);
        }
        
        [Fact]
        public void TestLibr()
        {
            var user = new UserDTO()
            {
                FullName = "Test",
                Login = "test",
                Password = "test",
                Role = "Библиотекарь"
            };
            Assert.Equal("Библиотекарь", user.Role);
        }

        [Fact]
        public void TestStock()
        {
            var user = new UserDTO()
            {
                FullName = "Test",
                Login = "test",
                Password = "test",
                Role = "Кладовщик"
            };
            Assert.Equal("Кладовщик", user.Role);
        }

        [Fact]
        public void TestBad()
        {
            var user = new UserDTO()
            {
                FullName = "Test",
                Login = "test",
                Password = "test",
                Role = "Челикс"
            };
            Assert.NotEqual("Челикс", user.Role);
        }
    }
}
