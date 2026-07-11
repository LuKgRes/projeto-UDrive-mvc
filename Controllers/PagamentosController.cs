using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Programacion_III.Data;
using Proyecto_Programacion_III;
using Proyecto_Programacion_III.Models.Entidades.Opciones;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Programacion_III.Controllers
{


    [Authorize(Roles = "Administrador,Mecanico")]
    public class PagamentosController : Controller
    {
        private readonly Context _context;

        public PagamentosController(Context context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var agendamentos = await _context.Agendamentos
                .Include(a => a.Cliente)
                .Include(a => a.Servicos)
                .OrderByDescending(a => a.Data)
                .ToListAsync();

            return View(agendamentos);
        }

        [HttpPost]
        public async Task<IActionResult> AlterarStatus(int id, StatusPagamento novoStatus)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null) return NotFound();

            agendamento.StatusPagamento = novoStatus;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }

}
