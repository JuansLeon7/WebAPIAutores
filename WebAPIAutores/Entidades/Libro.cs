﻿using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [Primera_LetraMayuscula]
        [StringLength(maximumLength:250)]
        public string Titulo { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }




    }
}
