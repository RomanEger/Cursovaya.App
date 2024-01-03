using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace CursovayaApp.WPF.Models.DbModels
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<DeregBook> DeregBooks { get; set; } = null!;
        public DbSet<Photo> Photos { get; set; } = null!;
        public DbSet<PublishingHouse> PublishingHouses { get; set; } = null!;
        public DbSet<ReasonDereg> ReasonsDereg { get; set; } = null!;
        public DbSet<ReasonReg> ReasonsReg { get; set; } = null!;
        public DbSet<RegBook> RegBooks { get; set; } = null!;
        public DbSet<RentalBook> RentalBooks { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public ApplicationContext()
            : base()
        {
            //удаление бд
            //Database.EnsureDeleted();

            //создание бд при ее отсутствии
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API
            base.OnModelCreating(modelBuilder);

            //Author
            modelBuilder.Entity<Author>().Property(x => x.FullName).HasMaxLength(120);
            modelBuilder.Entity<Author>().HasIndex(x => new { x.FullName, x.BirthYear, x.DeathYear }).IsUnique();
            modelBuilder.Entity<Author>().ToTable(x => x.HasCheckConstraint("BirthYear", "BirthYear<DeathYear"));
            modelBuilder.Entity<Author>().ToTable(x => x.HasCheckConstraint("DeathYear", "BirthYear<YEAR(GETDATE()) AND DeathYear<=YEAR(GETDATE())"));

            //Book
            modelBuilder.Entity<Book>().ToTable(x => x.HasCheckConstraint("Title", "LEN(Title)>0 AND Title<>''"));
            modelBuilder.Entity<Book>().ToTable(x => x.HasCheckConstraint("Quantity", "Quantity>=0"));
            modelBuilder.Entity<Book>().HasIndex(x => new{ x.Title, IdAuthor = x.AuthorId, IdPublishing = x.PublishingId}).IsUnique();

            //DeregBook
            modelBuilder.Entity<DeregBook>().Property(x => x.DateOfDereg).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<DeregBook>().ToTable(x => x.HasCheckConstraint("DeregQuantity", "DeregQuantity>=0"));

            //Photo
            modelBuilder.Entity<Photo>().HasIndex(x => x.Image).IsUnique();

            //PublishingHouse
            modelBuilder.Entity<PublishingHouse>().Property(x => x.Name).HasMaxLength(120);
            modelBuilder.Entity<PublishingHouse>().ToTable(x => x.HasCheckConstraint("Name", "LEN(Name)>0 AND Name<>''"));

            //ReasonDereg
            modelBuilder.Entity<ReasonDereg>().Property(x => x.Name).HasMaxLength(120);
            modelBuilder.Entity<ReasonDereg>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<ReasonDereg>().ToTable(x => x.HasCheckConstraint("Name", "LEN(Name)>0 AND Name<>''"));

            //ReasonReg
            modelBuilder.Entity<ReasonReg>().Property(x => x.Name).HasMaxLength(120);
            modelBuilder.Entity<ReasonReg>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<ReasonReg>().ToTable(x => x.HasCheckConstraint("Name", "LEN(Name)>0 AND Name<>''"));

            //RegBook
            modelBuilder.Entity<RegBook>().Property(x => x.DateOfReg).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<RegBook>().ToTable(x => x.HasCheckConstraint("RegQuantity", "RegQuantity>=0"));

            //RentalBook
            modelBuilder.Entity<RentalBook>().ToTable(x => x.HasCheckConstraint("DateStart", "DateStart<=DateEnd"));

            //Role
            modelBuilder.Entity<Role>().Property(x => x.Name).HasMaxLength(50);
            modelBuilder.Entity<Role>().ToTable(x => x.HasCheckConstraint("Name", "LEN(Name)>0 AND Name<>''"));
            modelBuilder.Entity<Role>().HasIndex(x => x.Name).IsUnique();

            //User
            //properties
            modelBuilder.Entity<User>().Property(x => x.FullName).HasMaxLength(120);
            modelBuilder.Entity<User>().Property(x => x.Login).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(x => x.Password).HasMaxLength(50);
            //check
            modelBuilder.Entity<User>().ToTable(x => x.HasCheckConstraint("FullName", "LEN(FullName) > 0 AND FullName <> ''"));
            modelBuilder.Entity<User>().ToTable(x => x.HasCheckConstraint("Login", "LEN(Login) >= 4 AND Login <> ''"));
            modelBuilder.Entity<User>().ToTable(x => x.HasCheckConstraint("Password", "LEN(Password) >= 4 AND Password <> ''"));
            //unique
            modelBuilder.Entity<User>().HasIndex(x => x.Login).IsUnique();

            //работает только при создании бд
            //SetData
            Role adm = new() { Id = 1, Name = "Администратор" };
            Role libr = new() { Id = 2, Name = "Библиотекарь" };
            Role stockMan = new() { Id = 3, Name = "Кладовщик" };
            Role client = new() { Id = 4, Name = "Клиент" };

            User admUser = new() { Id = 1, FullName = "СТАРТОВЫЙ АДМИНИСТРАТОР", Login = "_admin123", Password = "1234", RoleId = adm.Id };

            modelBuilder.Entity<Role>().HasData(adm, libr, stockMan, client);
            modelBuilder.Entity<User>().HasData(admUser);
        }
    }
}
