using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string PasswordHash { get; set; } // Store hashed passwords
}