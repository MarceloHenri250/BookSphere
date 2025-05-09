using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookSphere.Data;
using BookSphere.Models;

namespace BookSphere.Controllers
{
    public class LivrosController : Controller
    {
        private readonly BookSphereContext _context;

        // Construtor
        public LivrosController(BookSphereContext context)
        {
            _context = context;
        }

        // ==============================
        // Ações relacionadas à listagem
        // ==============================

        // GET: Livros
        public async Task<IActionResult> Index()
        {
            var livros = await _context.Livros
                .Include(l => l.Autor)
                .Include(l => l.Editora)
                .Include(l => l.Categoria)
                .Where(l => l.QuantidadeDisponivel > 0)
                .ToListAsync();

            return View(livros);
        }

        // GET: Livros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var livro = await _context.Livros
                .Include(l => l.Autor)
                .Include(l => l.Editora)
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (livro == null) return NotFound();

            // Verifica se o livro está nos favoritos do usuário autenticado
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var email = User.Identity.Name;
                var usuario = await _context.Usuarios
                    .Include(u => u.Favoritos)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (usuario != null)
                {
                    bool ehFavorito = usuario.Favoritos.Any(f => f.LivroId == livro.Id);
                    ViewBag.EhFavorito = ehFavorito;
                }
            }

            return View(livro);
        }

        // ==============================
        // Ações relacionadas à criação
        // ==============================

        // GET: Livros/Create
        public IActionResult Create()
        {
            CarregarDropdowns();
            return View();
        }

        // POST: Livros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Livro livro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(livro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CarregarDropdowns();
            return View(livro);
        }

        // ==============================
        // Ações relacionadas à edição
        // ==============================

        // GET: Livros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var livro = await _context.Livros.FindAsync(id);
            if (livro == null) return NotFound();

            CarregarDropdowns();
            return View(livro);
        }

        // POST: Livros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Livro livro)
        {
            if (id != livro.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(livro.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            CarregarDropdowns();
            return View(livro);
        }

        // ==============================
        // Ações relacionadas à exclusão
        // ==============================

        // GET: Livros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var livro = await _context.Livros
                .Include(l => l.Autor)
                .Include(l => l.Editora)
                .Include(l => l.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (livro == null) return NotFound();

            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro != null)
            {
                _context.Livros.Remove(livro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ==============================
        // Métodos auxiliares
        // ==============================

        private bool LivroExists(int id)
        {
            return _context.Livros.Any(e => e.Id == id);
        }

        private void CarregarDropdowns()
        {
            ViewData["AutorId"] = new SelectList(_context.Autores.OrderBy(a => a.Nome), "Id", "Nome");
            ViewData["EditoraId"] = new SelectList(_context.Editoras.OrderBy(e => e.Nome), "Id", "Nome");
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.OrderBy(c => c.Nome), "Id", "Nome");
        }
    }
}
