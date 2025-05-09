using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSphere.Models;
using Microsoft.Extensions.Logging;
using BookSphere.Data;
using BookSphere.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BookSphere.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookSphereContext _context;
        private readonly UserManager<Usuario> _userManager;

        // ==============================
        // Construtor
        // ==============================
        public HomeController(ILogger<HomeController> logger, BookSphereContext context, UserManager<Usuario> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // ==============================
        // Ações relacionadas à página inicial
        // ==============================

        // GET: Home/Index
        // Método para exibir a página inicial com filtros e ordenação
        public async Task<IActionResult> Index(string busca, int? categoriaId, int? autorId, int? editoraId, string ordem)
        {
            // Obtém a lista de livros com os relacionamentos necessários
            var livros = _context.Livros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .Include(l => l.Editora)
                .AsQueryable();

            // Filtro por termo de busca
            if (!string.IsNullOrEmpty(busca))
            {
                var termo = busca.ToLower();
                livros = livros.Where(l =>
                    l.Titulo.ToLower().Contains(termo) ||
                    l.Autor.Nome.ToLower().Contains(termo) ||
                    l.Categoria.Nome.ToLower().Contains(termo));
            }

            // Filtro por categoria
            if (categoriaId.HasValue)
                livros = livros.Where(l => l.CategoriaId == categoriaId.Value);

            // Filtro por autor
            if (autorId.HasValue)
                livros = livros.Where(l => l.AutorId == autorId.Value);

            // Filtro por editora
            if (editoraId.HasValue)
                livros = livros.Where(l => l.EditoraId == editoraId.Value);

            // Ordenação dos livros
            livros = ordem switch
            {
                "za" => livros.OrderByDescending(l => l.Titulo),
                _ => livros.OrderBy(l => l.Titulo),
            };

            // Carrega listas de categorias, autores e editoras para exibição
            ViewBag.Categorias = await _context.Categorias.OrderBy(c => c.Nome).ToListAsync();
            ViewBag.Autores = await _context.Autores.OrderBy(a => a.Nome).ToListAsync();
            ViewBag.Editoras = await _context.Editoras.OrderBy(e => e.Nome).ToListAsync();

            // Verifica se o usuário está autenticado e carrega os favoritos
            if (User.Identity.IsAuthenticated)
            {
                var usuario = await _userManager.GetUserAsync(User);
                var favoritos = await _context.Favoritos
                    .Where(f => f.UsuarioId == usuario.Id)
                    .Select(f => f.LivroId)
                    .ToListAsync();

                ViewBag.Favoritos = favoritos;
            }

            // Retorna a lista de livros para a view
            return View(await livros.ToListAsync());
        }

        // ==============================
        // Ações relacionadas à privacidade
        // ==============================

        // GET: Home/Privacy
        // Método para exibir a página de privacidade
        public IActionResult Privacy()
        {
            return View();
        }

        // ==============================
        // Ações relacionadas a erros
        // ==============================

        // GET: Home/Error
        // Método para exibir a página de erro
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
