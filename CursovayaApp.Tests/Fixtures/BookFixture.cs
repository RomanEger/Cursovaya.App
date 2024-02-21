using Bogus;
using CursovayaApp.WPF.Models.DbModels;

namespace CursovayaApp.Tests.Fixtures
{
    public class BookFixture : IFixture<Book>
    {
        public Faker<Book> GenerationRules() =>
        new Faker<Book>()
            .RuleFor(x => x.Id, r => r.IndexFaker)
            .RuleFor(x => x.Title, r => r.Random.Word())
            .RuleFor(x => x.AuthorId, r => r.Random.Int(1))
            .RuleFor(x => x.PublishingHouseId, r => r.Random.Int(1))
            .RuleFor(x => x.Quantity, r => r.Random.Int(1));

        public IEnumerable<Book> GetRandomData(int count) =>
            GenerationRules().Generate(count);

        public Faker<Author> GenerationRulesAuthor() =>
            new Faker<Author>()
                .RuleFor(x => x.Id, r => r.IndexFaker)
                .RuleFor(x => x.FullName, r => r.Random.Word())
                .RuleFor(x => x.BirthYear, r => r.Random.Int(-1000, 2020))
                .RuleFor(x => x.DeathYear, r => r.Random.Int(2020, 2024).OrNull(r));

        public IEnumerable<Author> GetRandomDataAuthor(int count) =>
            GenerationRulesAuthor().Generate(count);

        public IEnumerable<Book> GetTestData() =>
            new List<Book>()
            {
            new Book()
            {
                Id=1,
                AuthorId=1,
                PublishingHouseId=1,
                Quantity=55,
                Title = "Книга 1"
            },
            new Book()
            {
                Id=2,
                AuthorId=1,
                PublishingHouseId=1,
                Quantity=2135,
                Title = "Книга 2"
            },
            new Book()
            {
                Id=3,
                AuthorId=1,
                PublishingHouseId=1,
                Quantity=5,
                Title = "Книга 3"
            },
            new Book()
            {
                Id=4,
                AuthorId=2,
                PublishingHouseId=3,
                Quantity=598,
                Title = "Книга 4"
            }
            };

        public int Add(Book newBook)
        {
            var list = GetTestData().ToList();
            var authors = GetRandomDataAuthor(10).ToList();
            var count = list.Count;

            if (newBook.Title == null ||
               newBook.Id < 0 ||
               list.Any(x => x.Id == newBook.Id) ||
               string.IsNullOrEmpty(newBook.Title) ||
               !authors.Any(x => x.Id == newBook.AuthorId))
            {
                return 0;
            }

            list.Add(newBook);

            return list.Count - count;
        }

    }
}
