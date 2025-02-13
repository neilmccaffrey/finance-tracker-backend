using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Transaction
{
    [Key]
    public Guid Id { get; set; }

    [Required]
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
