using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSphere.Data;
using BookSphere.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookSphere.Controllers
{
    // Controlador protegido por autorização para o papel "Administrador"
    [Authorize(Roles = "Administrador")]
    public class CategoriasController : Controller
    {
        private readonly BookSphereContext _context;

        // ==============================
        // Construtor
        // ==============================
        public CategoriasController(BookSphereContext context)
        {
            _context = context;
        }

        // ==============================
        // Ações relacionadas à listagem
        // ==============================

        // GET: Categorias
        // Método para listar todas as categorias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias.ToListAsync());
        }

        // ==============================
        // Ações relacionadas aos detalhes
        // ==============================

        // GET: Categorias/Details/5
        // Método para exibir detalhes de uma categoria específica
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // ==============================
        // Ações relacionadas à criação
        // ==============================

        // GET: Categorias/Create
        // Método para exibir o formulário de criação de uma nova categoria
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // Método para processar a criação de uma nova categoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // ==============================
        // Ações relacionadas à edição
        // ==============================

        // GET: Categorias/Edit/5
        // Método para exibir o formulário de edição de uma categoria existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // Método para processar a edição de uma categoria existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Categoria categoria)
        {
            if (id != categoria.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // ==============================
        // Ações relacionadas à exclusão
        // ==============================

        // GET: Categorias/Delete/5
        // Método para exibir o formulário de exclusão de uma categoria
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        // Método para processar a exclusão de uma categoria
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ==============================
        // Métodos auxiliares
        // ==============================

        // Método auxiliar para verificar se uma categoria existe no banco de dados
        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
