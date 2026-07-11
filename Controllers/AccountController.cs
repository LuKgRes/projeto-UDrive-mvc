using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Data;
using Proyecto_Programacion_III.Models;
using Proyecto_Programacion_III.Models.Entidades;
using Proyecto_Programacion_III.Models.Entidades.Opciones;
using System;
using System.Security.Claims;

namespace Proyecto_Programacion_III.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _context;

        public AccountController(Context context) => _context = context;

        private string DefinirRolPorEmail(string email)
        {
            return email.Trim().EndsWith("@udrive.com.br", StringComparison.OrdinalIgnoreCase)
                ? "Mecanico"
                : "Cliente";
        }

        [HttpGet]
        public IActionResult Cadastro() => View();

        [HttpPost]
        public async Task<IActionResult> Cadastro(UsuarioCadastro model)
        {
            if (!ModelState.IsValid) return View(model);

            if (await _context.Usuarios.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                return View(model);
            }



            var usuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                Password = PasswordHasher.Hash(model.Password),
                Rol = DefinirRolPorEmail(model.Email),
                Estado = EstadoUsuario.Activo
            };

            _context.Usuarios.Add(usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (usuario == null || !PasswordHasher.Verify(model.Password, usuario.Password))
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos.");
                return View(model);
            }

            if (usuario.Estado != EstadoUsuario.Activo)
            {
                ModelState.AddModelError("", "Sua conta não está ativa. Contate o administrador.");
                return View(model);
            }

            if (usuario.Estado == EstadoUsuario.Suspendido)
            {
                ModelState.AddModelError("", "Sua conta está suspensa.");
                return View(model);
            }

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
