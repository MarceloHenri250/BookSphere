using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSphere.Models;
using BookSphere.Data;
using Microsoft.AspNetCore.Authorization;

namespace BookSphere.Controllers
{
    // Controlador protegido por autorização para o papel "Administrador"
    [Authorize(Roles = "Administrador")]
    public class EditorasController : Controller
    {
        private readonly BookSphereContext _context;

        // ==============================
        // Construtor
        // ==============================
        public EditorasController(BookSphereContext context)
        {
            _context = context;
        }

        // ==============================
        // Ações relacionadas à listagem
        // ==============================

        // GET: Editoras
        // Método para listar todas as editoras
        public async Task<IActionResult> Index()
        {
            return View(await _context.Editoras.ToListAsync());
        }

        // ==============================
        // Ações relacionadas aos detalhes
        // ==============================

        // GET: Editoras/Details/5
        // Método para exibir detalhes de uma editora específica
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var editora = await _context.Editoras.FirstOrDefaultAsync(m => m.Id == id);
            if (editora == null) return NotFound();

            return View(editora);
        }

        // ==============================
        // Ações relacionadas à criação
        // ==============================

        // GET: Editoras/Create
        // Método para exibir o formulário de criação de uma nova editora
        public IActionResult Create()
        {
            return View();
        }

        // POST: Editoras/Create
        // Método para processar a criação de uma nova editora
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Editora editora)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editora);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editora);
        }

        // ==============================
        // Ações relacionadas à edição
        // ==============================

        // GET: Editoras/Edit/5
        // Método para exibir o formulário de edição de uma editora existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var editora = await _context.Editoras.FindAsync(id);
            if (editora == null) return NotFound();

            return View(editora);
        }

        // POST: Editoras/Edit/5
        // Método para processar a edição de uma editora existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Editora editora)
        {
            if (id != editora.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editora);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditoraExists(editora.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editora);
        }

        // ==============================
        // Ações relacionadas à exclusão
        // ==============================

        // GET: Editoras/Delete/5
        // Método para exibir o formulário de exclusão de uma editora
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var editora = await _context.Editoras.FirstOrDefaultAsync(m => m.Id == id);
            if (editora == null) return NotFound();

            return View(editora);
        }

        // POST: Editoras/Delete/5
        // Método para processar a exclusão de uma editora
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var editora = await _context.Editoras.FindAsync(id);
            if (editora != null)
            {
                _context.Editoras.Remove(editora);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ==============================
        // Métodos auxiliares
        // ==============================

        // Método auxiliar para verificar se uma editora existe no banco de dados
        private bool EditoraExists(int id)
        {
            return _context.Editoras.Any(e => e.Id == id);
        }
    }
}
