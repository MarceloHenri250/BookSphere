using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookSphere.Data;
using BookSphere.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura o contexto do banco de dados
builder.Services.AddDbContext<BookSphereContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona Identity com suporte a roles
builder.Services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BookSphereContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura o pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Chama função que cria roles e admin padrão
SeedRolesAndAdminAsync(app).Wait();

app.Run();


// Função que faz seed de roles e cria admin
async Task SeedRolesAndAdminAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();

    string[] roles = { "Administrador", "Usuario" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Criar admin padrão se não existir
    var adminEmail = "admin@booksphere.com";
    var adminPassword = "Senha@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var novoAdmin = new Usuario
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            Nome = "Administrador"
        };

        var result = await userManager.CreateAsync(novoAdmin, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(novoAdmin, "Administrador");
        }
    }
}
