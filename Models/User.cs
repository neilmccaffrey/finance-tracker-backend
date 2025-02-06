using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public byte[] PasswordSalt { get; set; } // Store the salt (HMAC key)
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}