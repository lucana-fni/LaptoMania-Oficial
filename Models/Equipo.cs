using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LaptoManiaOficial.Models
{
    public class Equipo
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(6), MaxLength(10)]

        public string? Codigo { get; set; }

        public string? Marca { get; set; }

        [Display(Name = "Modelo")]
        public string? Modelo { get; set; }


        public decimal Precio { get; set; }

        public bool Disponibilidad { get; set; }
        public string? Foto { get; set; }

        //manejo de foto
        [NotMapped]
        [Display(Name = "Cargar Foto")]
        public IFormFile? FotoFile { get; set; }

    }
}
