using LaptoManiaOficial.Dtos;
using System.ComponentModel.DataAnnotations;

namespace LaptoManiaOficial.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(5), MaxLength(50)]
        public string? CorreoElectronico { get; set; }
        [Required, MinLength(5), MaxLength(50)]
        public string? Password { get; set; }
        [Required, MinLength(3), MaxLength(30)]
        public string? NombreCompleto { get; set; }
        public RolEnum Rol { get; set; }

        //relacion
        //public virtual List<Venta>? Ventas { get;set;}
    }
}
