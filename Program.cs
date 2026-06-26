using Bolao.Components;
using Bolao.Data;
using Bolao.Helpers;
using MudBlazor.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

// Configure SQLite DbContext
builder.Services.AddDbContext<BolaoDbContext>(options =>
    options.UseSqlite("Data Source=bolao.db"));

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "BolaoAuth";
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Run migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BolaoDbContext>();
    SeedData.Initialize(dbContext);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// Minimal API auth endpoints
app.MapPost("/api/auth/login", async (HttpContext context, BolaoDbContext dbContext) =>
{
    var email = context.Request.Form["email"].ToString();
    var password = context.Request.Form["password"].ToString();

    var hashedPassword = PasswordHasher.Hash(password);
    var user = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Senha == hashedPassword);

    if (user == null)
    {
        context.Response.Redirect("/login?error=InvalidCredentials");
        return;
    }

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Nome),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("IsAdmin", user.IsAdmin.ToString())
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    context.Response.Redirect("/meus-boloes");
}).DisableAntiforgery(); // Bypassing anti-forgery check for simplicity of standard HTML form post

app.MapGet("/api/auth/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/login");
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
