using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<Guid>(type: "TEXT", nullable: false),
                    CountryName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "TEXT", nullable: false),
                    PersonName = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    CountryID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { new Guid("1d7ba475-b6d1-49a2-af19-145c47bdd03f"), "India" },
                    { new Guid("29a38f00-c0c4-4359-876c-078ec8617cf7"), "Australia" },
                    { new Guid("abbca825-c334-4416-9bf3-64356f60ccd6"), "Canada" },
                    { new Guid("c01d8e8d-f6e8-404a-9c74-fcf869be2f16"), "UK" },
                    { new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"), "USA" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("0c4d0ef3-99a3-4ff7-97f4-e9e34296fa4c"), "Fieldstone Lane", new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"), new DateTime(1991, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "jasmina@hello.com", "Female", "Jasmina", false },
                    { new Guid("9e5f189a-d9a3-4a93-811f-4ceb06672a6c"), "Novick Terrace", new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"), new DateTime(1993, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "alledy@hello.com", "Male", "Aguste", false },
                    { new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"), "Sundown Point", new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"), new DateTime(1996, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "dbus@hello.com", "Female", "Dulcinea", false },
                    { new Guid("e983e477-1cae-4567-8842-7c1f2928d6e2"), "Pawling Alley", new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"), new DateTime(1993, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "kendall@hello.com", "Male", "Kendall", false },
                    { new Guid("ffdfd955-98b5-4f04-a312-0f36a4c9b7c7"), "Buhler Junction", new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"), new DateTime(1991, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "kilian@hello.com", "Male", "Kilian", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
