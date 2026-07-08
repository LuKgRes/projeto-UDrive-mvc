using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Data;
using Proyecto_Programacion_III.Models.Entidades;
using Proyecto_Programacion_III.Models.Entidades.Opciones;

public class AgendamentosController : Controller
{
    private readonly Context _context;

    public AgendamentosController(Context context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        var rol = HttpContext.Session.GetString("Rol");
        var usuarioId = HttpContext.Session.GetString("UsuarioId");

        var Agendamentos = _context.Agendamentos
            .Include(c => c.Cliente)
            .Include(c => c.Servicos)
            .Include(c => c.Usuario)
            .AsQueryable();

        if (rol == "Usuario" && usuarioId != null)
        {
            int id = int.Parse(usuarioId);
            Agendamentos = Agendamentos.Where(c => c.UsuarioId == id);
        }

        return View(await Agendamentos.ToListAsync());
    }

    public IActionResult Create()
    {
        ViewBag.Clientes = _context.Clientes.ToList();
        ViewBag.Servicoss = _context.Servicos.ToList();
        ViewBag.Usuarios = _context.Usuarios.ToList();
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(Agendamentos Agendamento)
    {
        Agendamento.Estado = EstadoAgendamentos.Programada;

        if (Agendamento.FechaHora < DateTime.Now)
        {
            ModelState.AddModelError("FechaHora", "No se pueden agendar Agendamentos en fechas pasadas");
        }
        var Servicos = _context.Servicos
        .FirstOrDefault(s => s.ServicosId == Agendamento.ServicosId);

        if (Servicos != null && Servicos.Estado == EstadoServicos.Inativo)
        {
            ModelState.AddModelError("ServicosId", "El Servicos se encuentra Inativo");
        }

        if (ModelState.IsValid)
        {
            _context.Agendamentos.Add(Agendamento);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewBag.Clientes = _context.Clientes.ToList();
        ViewBag.Servicoss = _context.Servicos.ToList();
        ViewBag.Usuarios = _context.Usuarios.ToList();

        return View(Agendamento);
    }
    public async Task<IActionResult> Edit(int id)
    {
        var Agendamento = await _context.Agendamentos.FindAsync(id);

        if (Agendamento == null)
            return NotFound();

        if (Agendamento.Estado == EstadoAgendamentos.Cancelada)
            return RedirectToAction("Index");

        ViewBag.Clientes = _context.Clientes.ToList();
        ViewBag.Servicoss = _context.Servicos.ToList();
        ViewBag.Usuarios = _context.Usuarios.ToList();

        return View(Agendamento);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Agendamentos Agendamento)
    {
        var AgendamentoDb = await _context.Agendamentos.FindAsync(Agendamento.Id);

        if (AgendamentoDb == null)
            return NotFound();

        if (AgendamentoDb.Estado == EstadoAgendamentos.Cancelada)
        {
            ModelState.AddModelError("", "No se puede editar una Agendamento cancelada");
        }

        if (Agendamento.FechaHora < DateTime.Now)
        {
            ModelState.AddModelError("FechaHora", "No se pueden usar fechas pasadas");
        }

        var Servicos = _context.Servicos
            .FirstOrDefault(s => s.ServicosId == Agendamento.ServicosId);

        if (Servicos != null && Servicos.Estado == EstadoServicos.Inativo)
        {
            ModelState.AddModelError("ServicosId", "Servicos Inativo");
        }

        if (ModelState.IsValid)
        {
            AgendamentoDb.ClienteId = Agendamento.ClienteId;
            AgendamentoDb.ServicosId = Agendamento.ServicosId;
            AgendamentoDb.UsuarioId = Agendamento.UsuarioId;
            AgendamentoDb.FechaHora = Agendamento.FechaHora;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewBag.Clientes = _context.Clientes.ToList();
        ViewBag.Servicoss = _context.Servicos.ToList();
        ViewBag.Usuarios = _context.Usuarios.ToList();

        return View(Agendamento);
    }

    public async Task<IActionResult> Cancelar(int id)
    {
        var Agendamento = await _context.Agendamentos.FindAsync(id);

        if (Agendamento != null)
        {
            Agendamento.Estado = EstadoAgendamentos.Cancelada;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var Agendamento = await _context.Agendamentos
            .Include(c => c.Cliente)
            .Include(c => c.Servicos)
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        return View(Agendamento);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var Agendamento = await _context.Agendamentos.FindAsync(id);

        if (Agendamento != null)
        {
            _context.Agendamentos.Remove(Agendamento);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}