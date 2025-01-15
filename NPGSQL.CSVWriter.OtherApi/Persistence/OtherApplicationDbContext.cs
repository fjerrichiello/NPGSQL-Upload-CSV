using Microsoft.EntityFrameworkCore;
using NPGSQL.CSVWriter.OtherApi.Persistence.Models;

namespace NPGSQL.CSVWriter.OtherApi.Persistence;

public class OtherApplicationDbContext : DbContext
{
    public virtual DbSet<OtherBook> OtherBooks { get; set; } = null!;

    public OtherApplicationDbContext(
        DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}