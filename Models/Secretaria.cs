using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LaptoManiaOficial.Models
{
    public class Secretaria
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(6), MaxLength(10)]
        public string? Ci { get; set; }
        [Required, MinLength(3), MaxLength(30)]
        [Display(Name = "Nombre de la Secretaria")]
        public string? NombreCompleto { get; set; }
        public string? Foto { get; set; }

        //manejo de foto
        [NotMapped]
        [Display(Name = "Cargar Foto")]
        public IFormFile? FotoFile { get; set; }

        //Relaciones
        public int UsuarioId { get; set;}
        public virtual Usuario? Usuario { get; set; }
    }
}
