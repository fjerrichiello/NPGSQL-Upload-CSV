using System.ComponentModel.DataAnnotations;

namespace NPGSQL.CSVWriter.OtherApi.Persistence.Models;

public class OtherBook
{
    [Key]
    public int Id { get; set; }

    public required string Title { get; set; }
}