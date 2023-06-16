using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Entidades;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class LibroCreacionDTO
    {
        [Primera_LetraMayuscula]
        [StringLength(maximumLength: 250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
