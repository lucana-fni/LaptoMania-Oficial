using System.ComponentModel.DataAnnotations;

namespace LaptoManiaOficial.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(6), MaxLength(10)]
        public string? Ci { get; set; }
        [Required, MinLength(3), MaxLength(30)]
        [Display(Name = "Nombre del cliente")]
        public string? NombreCompleto { get; set; } 
    }
}
