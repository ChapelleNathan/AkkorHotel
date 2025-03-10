using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Hotel
{
    [Key]
    public Guid Id { get; set; }
    
    
    [Column("name")]
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Column("location")]
    [Required]
    [MaxLength(50)]
    public string Location { get; set; }
    
    [Column("description")]
    [MaxLength(255)]
    public string Description { get; set; }
}