using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookSphere.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        // Foto de perfil opcional
        public string? FotoPerfil { get; set;}
        
        // Relacionamento com favoritos
        public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
    }
}
