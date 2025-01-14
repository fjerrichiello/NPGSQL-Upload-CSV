using System.ComponentModel.DataAnnotations;

namespace ConcurrencyPOC.Persistence.Models;

public class Author
{
    [Key]
    public int Id { get; set; }

    public required string AuthorId { get; set; }
}