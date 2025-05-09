using Microsoft.EntityFrameworkCore;
using BookSphere.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BookSphere.Data
{
    public class BookSphereContext : IdentityDbContext<Usuario>
    {
        public BookSphereContext(DbContextOptions<BookSphereContext> options)
            : base(options)
        {
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Editora> Editoras { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relações com Delete Restrito para impedir exclusão se estiver em uso
            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Livros)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Editora)
                .WithMany(e => e.Livros)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Livro>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Livros)
                .OnDelete(DeleteBehavior.Restrict);

            // Favoritos únicos por usuário e livro
            modelBuilder.Entity<Favorito>()
                .HasIndex(f => new { f.UsuarioId, f.LivroId })
                .IsUnique();
        }
    }
}
