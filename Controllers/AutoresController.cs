using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSphere.Data;
using BookSphere.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookSphere.Controllers
{
    // Controlador protegido por autorização para o papel "Administrador"
    [Authorize(Roles = "Administrador")]
    public class AutoresController : Controller
    {
        private readonly BookSphereContext _context;

        // ==============================
        // Construtor
        // ==============================
        public AutoresController(BookSphereContext context)
        {
            _context = context;
        }

        // ==============================
        // Ações relacionadas à listagem
        // ==============================

        // GET: Autores
        // Método para listar todos os autores
        public async Task<IActionResult> Index()
        {
            var autores = await _context.Autores.ToListAsync();
            return View(autores);
        }

        // ==============================
        // Ações relacionadas aos detalhes
        // ==============================

        // GET: Autores/Details/5
        // Método para exibir detalhes de um autor específico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var autor = await _context.Autores.FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null) return NotFound();

            return View(autor);
        }

        // ==============================
        // Ações relacionadas à criação
        // ==============================

        // GET: Autores/Create
        // Método para exibir o formulário de criação de um novo autor
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autores/Create
        // Método para processar a criação de um novo autor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // ==============================
        // Ações relacionadas à edição
        // ==============================

        // GET: Autores/Edit/5
        // Método para exibir o formulário de edição de um autor existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var autor = await _context.Autores.FindAsync(id);
            if (autor == null) return NotFound();

            return View(autor);
        }

        // POST: Autores/Edit/5
        // Método para processar a edição de um autor existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Autor autor)
        {
            if (id != autor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // ==============================
        // Ações relacionadas à exclusão
        // ==============================

        // GET: Autores/Delete/5
        // Método para exibir o formulário de exclusão de um autor
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var autor = await _context.Autores.FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null) return NotFound();

            return View(autor);
        }

        // POST: Autores/Delete/5
        // Método para processar a exclusão de um autor
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autores.FindAsync(id);

            // Verifica se o autor está associado a algum livro
            var estaEmUso = await _context.Livros.AnyAsync(l => l.AutorId == id);
            if (estaEmUso)
            {
                ModelState.AddModelError(string.Empty, "Este autor está associado a um ou mais livros e não pode ser excluído.");
                return View(autor);
            }

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ==============================
        // Métodos auxiliares
        // ==============================

        // Método auxiliar para verificar se um autor existe no banco de dados
        private bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.Id == id);
        }
    }
}
