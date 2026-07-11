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

            .AsQueryable();

        if (rol == "Usuario" && usuarioId != null)
        {
            int id = int.Parse(usuarioId);
            Agendamentos = Agendamentos.Where(c => c.ClienteId == id);
        }

        return View(await Agendamentos.ToListAsync());
    }

    public IActionResult Create()
    {
        ViewBag.Clientes = _context.Clientes.ToList();
        ViewBag.Servicos = _context.Servicos.ToList();
        ViewBag.Usuarios = _context.Usuarios.ToList();
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(Agendamentos Agendamento)
    {
        Agendamento.Estado = EstadoAgendamentos.Programada;

        if (Agendamento.Data < DateTime.Now)
        {
            ModelState.AddModelError("Data", "Não pode agendar para datas passadas.");
        }
        var Servicos = _context.Servicos
        .FirstOrDefault(s => s.ServicosId == Agendamento.ServicosId);

        if (Servicos != null && Servicos.Estado == EstadoServicos.Inativo)
        {
            ModelState.AddModelError("ServicosId", "O serviço está inativo.");
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
        ViewBag.Servicos = _context.Servicos.ToList();
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
            ModelState.AddModelError("", "Não pode editar um agendamento cancelado");
        }

        if (Agendamento.Data < DateTime.Now)
        {
            ModelState.AddModelError("Data", "Não pode usar datas passadas");
        }

        var Servicos = _context.Servicos
            .FirstOrDefault(s => s.ServicosId == Agendamento.ServicosId);

        if (Servicos != null && Servicos.Estado == EstadoServicos.Inativo)
        {
            ModelState.AddModelError("ServicosId", "Serviços Inativo");
        }

        if (ModelState.IsValid)
        {
            AgendamentoDb.ClienteId = Agendamento.ClienteId;
            AgendamentoDb.ServicosId = Agendamento.ServicosId;
            AgendamentoDb.Data = Agendamento.Data;

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


    public IActionResult CreatePersonalizado()
    {
        ViewBag.Clientes = _context.Clientes.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePersonalizado(AgendamentoPersonalizado model)
    {
        if (model.Data < DateTime.Now)
        {
            ModelState.AddModelError("Data", "Não se pode agendar Agendamentos em datas passadas");
        }

        if (ModelState.IsValid)
        {
            var novoServico = new Servicos
            {
                Nome = model.ServicoNome,
                Descricao = model.ServicoDescricao,
                Valor = model.ServicoValor,
                Tempo = model.Data,
                Estado = EstadoServicos.Ativo,
                Personalizado = true
            };

            _context.Servicos.Add(novoServico);
            await _context.SaveChangesAsync();

            var agendamento = new Agendamentos
            {
                ClienteId = model.ClienteId,
                ServicosId = novoServico.ServicosId,
                Data = model.Data,
                Estado = EstadoAgendamentos.Programada
            };

            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Clientes = _context.Clientes.ToList();
        return View(model);
    }

}