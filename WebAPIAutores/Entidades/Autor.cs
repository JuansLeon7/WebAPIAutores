using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Entidades
{
    public class Autor 
    {
        public int id { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe de tener más de {1} carácteres")]
        [Primera_LetraMayuscula]
        public string name { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }

    }
}
