using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Transaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]  // Make sure this matches the database column name
    public int UserId { get; set; }  // map to user_id in the database

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Type { get; set; } 

    [Required]
    public DateTime Date { get; set; } = DateTime.Now;
}
