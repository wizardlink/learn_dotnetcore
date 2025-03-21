using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options) { }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().ToTable("Countries");
        modelBuilder.Entity<Person>().ToTable("Persons");

        // Seed to countries
        var countries =
            JsonSerializer.Deserialize<List<Country>>(File.ReadAllText("countries.json")) ?? throw new JsonException();

        foreach (var country in countries)
        {
            modelBuilder.Entity<Country>().HasData(country);
        }

        // Seed to countries
        var persons =
            JsonSerializer.Deserialize<List<Person>>(File.ReadAllText("persons.json")) ?? throw new JsonException();

        foreach (var person in persons)
        {
            modelBuilder.Entity<Person>().HasData(person);
        }

        // Fluent API
        modelBuilder
            .Entity<Person>()
            .Property(tbl => tbl.TIN)
            .HasColumnName("TaxIdentificationNumber")
            .HasColumnType("TEXT")
            .HasDefaultValue("ABC12345");

        // modelBuilder
        //     .Entity<Person>()
        //     .HasIndex(tbl => tbl.TIN)
        //     .IsUnique();

        modelBuilder
            .Entity<Person>()
            .ToTable(tbl => tbl.HasCheckConstraint("CHK_TIN", "length([TaxIdentificationNumber]) = 8"));

        modelBuilder.Entity<Person>(entity =>
        {
            entity
                .HasOne<Country>(e => e.Country)
                .WithMany(country => country.Persons)
                .HasForeignKey(country => country.CountryID);
        });
    }
}
