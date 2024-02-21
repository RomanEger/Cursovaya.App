using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CursovayaApp.WPF.Migrations
{
    /// <inheritdoc />
    public partial class AlterTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    BirthYear = table.Column<int>(type: "int", nullable: false),
                    DeathYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.CheckConstraint("BirthYear", "BirthYear<DeathYear");
                    table.CheckConstraint("DeathYear", "BirthYear<YEAR(GETDATE()) AND DeathYear<=YEAR(GETDATE())");
                });

            migrationBuilder.CreateTable(
                name: "PublishingHouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublishingHouses", x => x.Id);
                    table.CheckConstraint("Name", "LEN(Name)>0 AND Name<>''");
                });

            migrationBuilder.CreateTable(
                name: "ReasonsDereg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonsDereg", x => x.Id);
                    table.CheckConstraint("Name1", "LEN(Name)>0 AND Name<>''");
                });

            migrationBuilder.CreateTable(
                name: "ReasonsReg",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonsReg", x => x.Id);
                    table.CheckConstraint("Name2", "LEN(Name)>0 AND Name<>''");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.CheckConstraint("Name3", "LEN(Name)>0 AND Name<>''");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    PublishingHouseId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.CheckConstraint("Quantity", "Quantity>=0");
                    table.CheckConstraint("Title", "LEN(Title)>0 AND Title<>''");
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_PublishingHouses_PublishingHouseId",
                        column: x => x.PublishingHouseId,
                        principalTable: "PublishingHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.CheckConstraint("FullName", "LEN(FullName) > 0 AND FullName <> ''");
                    table.CheckConstraint("Login", "LEN(Login) >= 4 AND Login <> ''");
                    table.CheckConstraint("Password", "LEN(Password) >= 4 AND Password <> ''");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(900)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeregBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    ReasonId = table.Column<int>(type: "int", nullable: false),
                    DateOfDereg = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DeregQuantity = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeregBooks", x => x.Id);
                    table.CheckConstraint("DeregQuantity", "DeregQuantity>=0");
                    table.ForeignKey(
                        name: "FK_DeregBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeregBooks_ReasonsDereg_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "ReasonsDereg",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeregBooks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    ReasonId = table.Column<int>(type: "int", nullable: false),
                    DateOfReg = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    RegQuantity = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegBooks", x => x.Id);
                    table.CheckConstraint("RegQuantity", "RegQuantity>=0");
                    table.ForeignKey(
                        name: "FK_RegBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegBooks_ReasonsReg_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "ReasonsReg",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegBooks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentalBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRentalEnd = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalBooks", x => x.Id);
                    table.CheckConstraint("DateStart", "DateStart<=DateEnd");
                    table.ForeignKey(
                        name: "FK_RentalBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalBooks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthYear", "DeathYear", "FullName" },
                values: new object[] { 1, 1799, 1837, "Пушкин Александр Сергеевич" });

            migrationBuilder.InsertData(
                table: "PublishingHouses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "АСТ" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Администратор" },
                    { 2, "Библиотекарь" },
                    { 3, "Кладовщик" },
                    { 4, "Клиент" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "Login", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, "СТАРТОВЫЙ АДМИНИСТРАТОР", "_admin123", "1234", 1 },
                    { 2, "СТАРТОВЫЙ БИБЛИОТЕКАРЬ", "_libr123", "1234", 2 },
                    { 3, "СТАРТОВЫЙ КЛАДОВЩИК", "_stock123", "1234", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_FullName",
                table: "Authors",
                column: "FullName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublishingHouseId",
                table: "Books",
                column: "PublishingHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Title_AuthorId_PublishingHouseId",
                table: "Books",
                columns: new[] { "Title", "AuthorId", "PublishingHouseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeregBooks_BookId",
                table: "DeregBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_DeregBooks_ReasonId",
                table: "DeregBooks",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_DeregBooks_UserId",
                table: "DeregBooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_BookId",
                table: "Photos",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_Image",
                table: "Photos",
                column: "Image",
                unique: true,
                filter: "[Image] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PublishingHouses_Name",
                table: "PublishingHouses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReasonsDereg_Name",
                table: "ReasonsDereg",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReasonsReg_Name",
                table: "ReasonsReg",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegBooks_BookId",
                table: "RegBooks",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_RegBooks_ReasonId",
                table: "RegBooks",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_RegBooks_UserId",
                table: "RegBooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalBooks_BookId_DateStart",
                table: "RentalBooks",
                columns: new[] { "BookId", "DateStart" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalBooks_UserId",
                table: "RentalBooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeregBooks");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "RegBooks");

            migrationBuilder.DropTable(
                name: "RentalBooks");

            migrationBuilder.DropTable(
                name: "ReasonsDereg");

            migrationBuilder.DropTable(
                name: "ReasonsReg");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "PublishingHouses");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
