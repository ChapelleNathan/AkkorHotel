using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Backend.Models;

public class Hotel(string name, string location, string? description)
{
    [Key]
    public Guid Id { get; set; }


    [Column("name")]
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = name;
    
    [Column("location")]
    [Required]
    [MaxLength(100)]
    public string Location { get; set; } = location;
    
    [Column("description")]
    [MaxLength(255)]
    public string? Description { get; set; } = description;
}