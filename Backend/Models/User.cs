using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class User(string firstname, string lastname, string email, string password)
{
    [Key]
    public Guid Id { get; set; }

    [Column("firstname")]
    [Required]
    [MaxLength(100)]
    public string Firstname { get; set; } = firstname;

    [Column("lastname")]
    [Required]
    [MaxLength(100)]
    public string Lastname { get; set; } = lastname;

    [Column("email")]
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = email;

    [Column("password")]
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = password;
}