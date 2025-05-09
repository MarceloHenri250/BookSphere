using BookSphere.Models;
using BookSphere.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookSphere.Controllers
{
    // Controlador protegido por autorização para o papel "Administrador"
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // ==============================
        // Construtor
        // ==============================
        public UsuariosController(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ==============================
        // Ações relacionadas à listagem de usuários
        // ==============================

        // GET: Usuarios/Index
        // Método para listar todos os usuários e seus papéis
        public async Task<IActionResult> Index()
        {
            // Obtém a lista de usuários
            var usuarios = _userManager.Users.ToList();
            var lista = new List<UsuarioComRoleViewModel>();

            // Monta a lista de usuários com seus papéis
            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                lista.Add(new UsuarioComRoleViewModel
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Role = roles.FirstOrDefault() ?? "Nenhuma"
                });
            }

            // Carrega a lista de papéis disponíveis para exibição
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();

            // Retorna a view com a lista de usuários
            return View(lista);
        }

        // ==============================
        // Ações relacionadas à alteração de papéis
        // ==============================

        // POST: Usuarios/AlterarPapel
        // Método para alterar o papel de um usuário
        [HttpPost]
        public async Task<IActionResult> AlterarPapel(string userId, string novoPapel)
        {
            // Obtém o usuário pelo ID
            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null)
            {
                return NotFound();
            }

            // Remove os papéis atuais do usuário
            var papeisAtuais = await _userManager.GetRolesAsync(usuario);
            await _userManager.RemoveFromRolesAsync(usuario, papeisAtuais);

            // Adiciona o novo papel ao usuário
            await _userManager.AddToRoleAsync(usuario, novoPapel);

            // Redireciona para a lista de usuários
            return RedirectToAction(nameof(Index));
        }
    }
}
