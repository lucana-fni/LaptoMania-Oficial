using LaptoManiaOficial.Contexto;
using LaptoManiaOficial.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Policy;

namespace LaptoManiaOficial.Controllers
{
    public class LoginController : Controller
    {
        private MiContext _context;

        public LoginController(MiContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {

            var usuario = await _context.Usuarios
                                    .Where(x => x.CorreoElectronico == email && x.Password == password)
                                    .FirstOrDefaultAsync();
            if (usuario != null)
            {
                await SetUserCookie(usuario);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["LoginError"] = "Correo electronico o password INCORRECTO";
                return RedirectToAction("Index");
            }

        }

        private async Task SetUserCookie(Usuario usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario!.NombreCompleto!),
                new Claim(ClaimTypes.Email, usuario!.CorreoElectronico!),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol!.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
