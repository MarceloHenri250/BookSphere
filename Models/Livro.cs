using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookSphere.Models;

namespace BookSphere.Models
{
    public class Livro
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = null!;

        [Required]
        public int AutorId { get; set; }

        [ForeignKey("AutorId")]
        public Autor? Autor { get; set; }

        [Required]
        public int EditoraId { get; set; }

        [ForeignKey("EditoraId")]
        public Editora? Editora { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }

        [Required]
        [Range(1000, 9999)]
        public int AnoPublicacao { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "O número de páginas deve ser maior que zero.")]
        public int Paginas { get; set; }

        [StringLength(1000)]
        public string? Descricao { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int QuantidadeDisponivel { get; set; }

        [NotMapped]
        public bool Disponivel => QuantidadeDisponivel > 0;

        [Url]
        public string? CapaUrl { get; set; }

        [Required, StringLength(20)]
        public string? ISBN { get; set; }

        public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
    }
}
