// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250219204037_TINColumn")]
    partial class TINColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Entities.Country", b =>
                {
                    b.Property<Guid>("CountryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryName")
                        .HasColumnType("TEXT");

                    b.HasKey("CountryID");

                    b.ToTable("Countries", (string)null);

                    b.HasData(
                        new
                        {
                            CountryID = new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"),
                            CountryName = "USA"
                        },
                        new
                        {
                            CountryID = new Guid("abbca825-c334-4416-9bf3-64356f60ccd6"),
                            CountryName = "Canada"
                        },
                        new
                        {
                            CountryID = new Guid("c01d8e8d-f6e8-404a-9c74-fcf869be2f16"),
                            CountryName = "UK"
                        },
                        new
                        {
                            CountryID = new Guid("1d7ba475-b6d1-49a2-af19-145c47bdd03f"),
                            CountryName = "India"
                        },
                        new
                        {
                            CountryID = new Guid("29a38f00-c0c4-4359-876c-078ec8617cf7"),
                            CountryName = "Australia"
                        });
                });

            modelBuilder.Entity("Entities.Person", b =>
                {
                    b.Property<Guid>("PersonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CountryID")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("ReceiveNewsLetters")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TIN")
                        .HasColumnType("TEXT");

                    b.HasKey("PersonID");

                    b.ToTable("Persons", (string)null);

                    b.HasData(
                        new
                        {
                            PersonID = new Guid("9e5f189a-d9a3-4a93-811f-4ceb06672a6c"),
                            Address = "Novick Terrace",
                            CountryID = new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"),
                            DateOfBirth = new DateTime(1993, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "alledy@hello.com",
                            Gender = "Male",
                            PersonName = "Aguste",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonID = new Guid("0c4d0ef3-99a3-4ff7-97f4-e9e34296fa4c"),
                            Address = "Fieldstone Lane",
                            CountryID = new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"),
                            DateOfBirth = new DateTime(1991, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jasmina@hello.com",
                            Gender = "Female",
                            PersonName = "Jasmina",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonID = new Guid("e983e477-1cae-4567-8842-7c1f2928d6e2"),
                            Address = "Pawling Alley",
                            CountryID = new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"),
                            DateOfBirth = new DateTime(1993, 8, 13, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "kendall@hello.com",
                            Gender = "Male",
                            PersonName = "Kendall",
                            ReceiveNewsLetters = false
                        },
                        new
                        {
                            PersonID = new Guid("ffdfd955-98b5-4f04-a312-0f36a4c9b7c7"),
                            Address = "Buhler Junction",
                            CountryID = new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"),
                            DateOfBirth = new DateTime(1991, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "kilian@hello.com",
                            Gender = "Male",
                            PersonName = "Kilian",
                            ReceiveNewsLetters = true
                        },
                        new
                        {
                            PersonID = new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"),
                            Address = "Sundown Point",
                            CountryID = new Guid("ccbe8e62-6081-4072-b36d-fa7987d50000"),
                            DateOfBirth = new DateTime(1996, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "dbus@hello.com",
                            Gender = "Female",
                            PersonName = "Dulcinea",
                            ReceiveNewsLetters = false
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
