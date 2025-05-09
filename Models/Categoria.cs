using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookSphere.Models;

namespace BookSphere.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = null!;

        public ICollection<Livro> Livros { get; set; } = new List<Livro>();
    }
}