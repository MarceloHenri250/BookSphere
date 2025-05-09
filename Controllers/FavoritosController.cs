using BookSphere.Data;
using BookSphere.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSphere.Controllers
{
    // Controlador protegido por autorização para gerenciar favoritos
    [Authorize]
    public class FavoritosController : Controller
    {
        private readonly BookSphereContext _context;
        private readonly UserManager<Usuario> _userManager;

        // ==============================
        // Construtor
        // ==============================
        public FavoritosController(BookSphereContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ==============================
        // Ações relacionadas à adição de favoritos
        // ==============================

        // POST: Favoritos/Adicionar
        // Método para adicionar um livro aos favoritos do usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adicionar(int livroId)
        {
            // Obtém o ID do usuário logado
            var userId = _userManager.GetUserId(User);

            // Verifica se o livro já está nos favoritos do usuário
            bool jaFavoritado = await _context.Favoritos
                .AnyAsync(f => f.UsuarioId == userId && f.LivroId == livroId);

            if (jaFavoritado)
            {
                TempData["Mensagem"] = "Este livro já está nos seus favoritos.";
                return RedirectToAction("Index", "Home"); // Redireciona para a Home
            }

            // Cria um novo registro de favorito
            var favorito = new Favorito
            {
                UsuarioId = userId,
                LivroId = livroId
            };

            // Adiciona o favorito ao banco de dados
            _context.Favoritos.Add(favorito);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Livro adicionado aos favoritos!";
            return RedirectToAction("Index", "Home"); // Redireciona para a Home
        }

        // ==============================
        // Ações relacionadas à remoção de favoritos
        // ==============================

        // POST: Favoritos/Remover
        // Método para remover um livro dos favoritos do usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remover(int livroId)
        {
            // Obtém o ID do usuário logado
            var userId = _userManager.GetUserId(User);

            // Busca o registro de favorito no banco de dados
            var favorito = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == userId && f.LivroId == livroId);

            if (favorito == null)
            {
                TempData["Mensagem"] = "Este livro não está nos seus favoritos.";
                return RedirectToAction("MinhaBiblioteca", "Perfil"); // Redireciona para a biblioteca do usuário
            }

            // Remove o favorito do banco de dados
            _context.Favoritos.Remove(favorito);
            await _context.SaveChangesAsync();

            TempData["Mensagem"] = "Livro removido dos favoritos.";
            return RedirectToAction("MinhaBiblioteca", "Perfil"); // Redireciona para a biblioteca do usuário
        }
    }
}
