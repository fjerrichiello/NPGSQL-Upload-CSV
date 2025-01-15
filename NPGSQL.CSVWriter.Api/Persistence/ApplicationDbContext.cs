using Microsoft.EntityFrameworkCore;
using NPGSQL.CSVWriter.Api.Persistence.Models;

namespace NPGSQL.CSVWriter.Api.Persistence;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Author> Authors { get; set; } = null!;

    public virtual DbSet<Book> Books { get; set; } = null!;
    
    public ApplicationDbContext(
        DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>()
            .HasIndex(b => new { b.AuthorId, b.Title })
            .IsUnique();

        List<Author> authors =
        [
            new Author() { Id = 1, AuthorId = "Dr. Seuss" },
            new Author() { Id = 2, AuthorId = "Roald Dahl" },
            new Author() { Id = 3, AuthorId = "Beatrix Potter" },
            new Author() { Id = 4, AuthorId = "Maurice Sendak" },
            new Author() { Id = 5, AuthorId = "Eric Carle" },
            new Author() { Id = 6, AuthorId = "Shel Silverstein" },
            new Author() { Id = 7, AuthorId = "Judy Blume" }
        ];
        
        modelBuilder.Entity<Author>()
            .HasData(authors);
    }
}