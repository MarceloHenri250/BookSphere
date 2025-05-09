using BookSphere.Data;
using BookSphere.Models;
using BookSphere.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSphere.Controllers
{
    // Controlador protegido por autorização para gerenciar o perfil do usuário
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly BookSphereContext _context;
        private readonly UserManager<Usuario> _userManager;

        // ==============================
        // Construtor
        // ==============================
        public PerfilController(BookSphereContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ==============================
        // Ações relacionadas à biblioteca do usuário
        // ==============================

        // GET: Perfil/MinhaBiblioteca
        // Método para exibir a biblioteca do usuário com seus livros favoritos
        public async Task<IActionResult> MinhaBiblioteca()
        {
            // Obtém o usuário logado
            var usuario = await _userManager.GetUserAsync(User);

            // Verifica se o usuário foi encontrado
            if (usuario == null)
            {
                return NotFound();
            }

            // Obtém os livros favoritos do usuário
            var favoritos = await _context.Favoritos
                .Include(f => f.Livro) // Inclui os detalhes do livro
                .Where(f => f.UsuarioId == usuario.Id)
                .ToListAsync();

            // Cria o modelo de dados para a view
            var model = new MinhaBibliotecaViewModel
            {
                NomeUsuario = usuario.Nome,
                Favoritos = favoritos
            };

            // Retorna a view com o modelo
            return View(model);
        }
    }
}
