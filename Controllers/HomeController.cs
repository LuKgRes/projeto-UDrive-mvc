using Microsoft.AspNetCore.Mvc;
using Proyecto_Programacion_III.Data;
using Proyecto_Programacion_III.Models;
using Proyecto_Programacion_III.Models.Entidades.Opciones;
using System.Diagnostics;

namespace Proyecto_Programacion_III.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult LoginAdmin()
        {
            HttpContext.Session.SetString("Rol", "Administrador");
            return RedirectToAction("Index");
        }


        public IActionResult LoginUsuario()
        {
            HttpContext.Session.SetString("Rol", "Usuario");
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult DashboardAdmin()
        {

            ViewBag.TotalUsuarios = _context.Usuarios.Count();
            ViewBag.TotalClientes = _context.Clientes.Count();
            ViewBag.TotalAgendamentos = _context.Agendamentos.Count();


            ViewBag.ServicossActivos = _context.Servicos
                .Count(s => s.Estado == EstadoServicos.Activo);

            ViewBag.ServicossInativos = _context.Servicos
                .Count(s => s.Estado == EstadoServicos.Inativo);

            ViewBag.AgendamentosProgramadas = _context.Agendamentos
                .Count(c => c.Estado == EstadoAgendamentos.Programada);

            ViewBag.AgendamentosCanceladas = _context.Agendamentos
                .Count(c => c.Estado == EstadoAgendamentos.Cancelada);


            var topServicoss = _context.Agendamentos
                .GroupBy(c => c.Servicos.Nome)
                .Select(g => new
                {
                    Servicos = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .Take(3)
                .ToList();

            ViewBag.TopServicoss = topServicoss;

            return View();
        }

        public IActionResult DashboardUsuario()
        {

            ViewBag.TotalAgendamentos = _context.Agendamentos.Count();

            ViewBag.ServicossActivos = _context.Servicos
               .Count(s => s.Estado == EstadoServicos.Activo);

            ViewBag.ServicossInativos = _context.Servicos
                .Count(s => s.Estado == EstadoServicos.Inativo);

            ViewBag.AgendamentosProgramadas = _context.Agendamentos
                .Count(c => c.Estado == EstadoAgendamentos.Programada);

            ViewBag.AgendamentosCanceladas = _context.Agendamentos
                .Count(c => c.Estado == EstadoAgendamentos.Cancelada);

            ViewBag.Agendamentos = _context.Agendamentos.ToList();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}