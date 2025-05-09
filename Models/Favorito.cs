using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSphere.Models
{
    public class Favorito
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } 
        public Usuario Usuario { get; set; }

        [Required]
        public int LivroId { get; set; }

        public Livro Livro { get; set; }
    }
}
