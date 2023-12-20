using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LaptoManiaOficial.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime FechaVenta { get; set; }
       
        // Relaciones
        public int UsuarioId { get; set; }
        public virtual Usuario? Usuario { get; set; }

        public int ClienteId { get; set; }
        public virtual Cliente? Cliente { get; set; }

        public int EquipoId { get; set; }
        public virtual Equipo? Equipo { get; set; }
    }
}
