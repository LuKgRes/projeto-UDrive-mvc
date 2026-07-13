using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Data;
using Proyecto_Programacion_III.Models.Entidades;
using Proyecto_Programacion_III.Models.Entidades.Opciones;
using System.Security.Claims;

public class AgendamentosController : Controller
{
    private readonly Context _context;

    public AgendamentosController(Context context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        var rol = User.FindFirstValue(ClaimTypes.Role);
        var usuarioIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);


        var Agendamentos = _context.Agendamentos
            .Include(c => c.Cliente)
            .Include(c => c.Servicos)
            .AsQueryable();

        if (rol == "Cliente" && usuarioIdStr != null)
        {
            int id = int.Parse(usuarioIdStr);
            Agendamentos = Agendamentos.Where(a => a.Cliente.UsuarioId == id);
        }

        return View(await Agendamentos.ToListAsync());
    }

    public IActionResult Create()
    {
        var rol = User.FindFirstValue(ClaimTypes.Role);
        var usuarioIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var clientesQuery = _context.Clientes.AsQueryable();
        var veiculosQuery = _context.Veiculos.AsQueryable();

        if (rol == "Cliente" && usuarioIdStr != null)
        {
            int id = int.Parse(usuarioIdStr);
            clientesQuery = clientesQuery.Where(c => c.UsuarioId == id);
            veiculosQuery = veiculosQuery.Where(v => clientesQuery.Select(c => c.ClienteId).Contains(v.ClienteId));
        }

        ViewBag.Clientes = clientesQuery.ToList();
        ViewBag.Veiculos = veiculosQuery.ToList();
        ViewBag.Servicos = _context.Servicos.ToList();
        ViewBag.Usuarios = _context.Usuarios.ToList();

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(
    Agendamentos Agendamento,
   string VeiculoSelecionado,
    string NovoVeiculoModelo,
    string NovoVeiculoPlaca,
    string NovoVeiculoMarca,
    string NovoVeiculoCor,
    int? NovoVeiculoAno)
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

        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var clientePertenceAoUsuario = await _context.Clientes
            .AnyAsync(c => c.ClienteId == Agendamento.ClienteId && c.UsuarioId == usuarioId);

        if (!clientePertenceAoUsuario)
        {
            ModelState.AddModelError("ClienteId", "Cliente inválido.");
        }

        bool isNovoVeiculo = VeiculoSelecionado == "novo";

        if (isNovoVeiculo)
        {
            if (string.IsNullOrWhiteSpace(NovoVeiculoModelo))
                ModelState.AddModelError("NovoVeiculoModelo", "Informe o modelo do veículo.");
            if (string.IsNullOrWhiteSpace(NovoVeiculoPlaca))
                ModelState.AddModelError("NovoVeiculoPlaca", "Informe a placa do veículo.");
        }
        else if (!int.TryParse(VeiculoSelecionado, out _))
        {
            ModelState.AddModelError("VeiculoId", "Selecione um veículo.");
        }


        ModelState.Remove("VeiculoId");
        ModelState.Remove("Agendamento.VeiculoId");
        ModelState.Remove("Veiculo");
        ModelState.Remove("Agendamento.Veiculo");

        if (ModelState.IsValid)
        {
            if (isNovoVeiculo)
            {
                var novoVeiculo = new Veiculo
                {
                    ClienteId = Agendamento.ClienteId,
                    Modelo = NovoVeiculoModelo,
                    Placa = NovoVeiculoPlaca,
                    Marca = NovoVeiculoMarca,
                    Cor = NovoVeiculoCor,
                    Ano = NovoVeiculoAno
                };

                _context.Veiculos.Add(novoVeiculo);
                await _context.SaveChangesAsync();

                Agendamento.VeiculoId = novoVeiculo.VeiculoId;
            }
            else
            {
                Agendamento.VeiculoId = int.Parse(VeiculoSelecionado);
            }

            _context.Agendamentos.Add(Agendamento);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewBag.Clientes = _context.Clientes.ToList();
        ViewBag.Servicos = _context.Servicos.ToList();
        ViewBag.Usuarios = _context.Usuarios.ToList();
        ViewBag.Veiculos = _context.Veiculos.ToList();

        return View(Agendamento);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var agendamento = await _context.Agendamentos
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .Include(a => a.Servicos)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (agendamento == null) return NotFound();

        return View(agendamento);
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
        var rol = User.FindFirstValue(ClaimTypes.Role);
        var usuarioIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var clientesQuery = _context.Clientes.AsQueryable();
        var veiculosQuery = _context.Veiculos.AsQueryable();

        if (rol == "Cliente" && usuarioIdStr != null)
        {
            int id = int.Parse(usuarioIdStr);
            clientesQuery = clientesQuery.Where(c => c.UsuarioId == id);
            veiculosQuery = veiculosQuery.Where(v => v.ClienteId != null && clientesQuery.Select(c => c.ClienteId).Contains(v.ClienteId));
        }

        ViewBag.Clientes = clientesQuery.ToList();
        ViewBag.Veiculos = veiculosQuery.ToList();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePersonalizado(
        AgendamentoPersonalizado model,
    string? VeiculoSelecionado,
    string? NovoVeiculoModelo,
    string? NovoVeiculoPlaca,
    string? NovoVeiculoMarca,
    string? NovoVeiculoCor,
    int? NovoVeiculoAno)
    {
        if (model.Data < DateTime.Now)
        {
            ModelState.AddModelError("Data", "Não se pode agendar Agendamentos em datas passadas");
        }

        bool isNovoVeiculo = VeiculoSelecionado == "novo";

        if (isNovoVeiculo)
        {
            if (string.IsNullOrWhiteSpace(NovoVeiculoModelo))
                ModelState.AddModelError("NovoVeiculoModelo", "Informe o modelo do veículo.");
            if (string.IsNullOrWhiteSpace(NovoVeiculoPlaca))
                ModelState.AddModelError("NovoVeiculoPlaca", "Informe a placa do veículo.");
        }
        else if (!int.TryParse(VeiculoSelecionado, out _))
        {
            ModelState.AddModelError("VeiculoSelecionado", "Selecione um veículo.");
        }

       
        ModelState.Remove("ServicoNome");
        ModelState.Remove("ServicoValor");

        if (ModelState.IsValid)
        {
            int veiculoIdFinal;

            if (isNovoVeiculo)
            {
                var novoVeiculo = new Veiculo
                {
                    ClienteId = model.ClienteId,
                    Modelo = NovoVeiculoModelo,
                    Placa = NovoVeiculoPlaca,
                    Marca = NovoVeiculoMarca,
                    Cor = NovoVeiculoCor,
                    Ano = NovoVeiculoAno
                };

                _context.Veiculos.Add(novoVeiculo);
                await _context.SaveChangesAsync();

                veiculoIdFinal = novoVeiculo.VeiculoId;
            }
            else
            {
                veiculoIdFinal = int.Parse(VeiculoSelecionado);
            }

            var novoServico = new Servicos
            {
                Nome = $"Personalizado - {model.Data:dd/MM/yyyy}",
                Descricao = model.ServicoDescricao,
                Valor = 0,
                Tempo = model.Data,
                Estado = EstadoServicos.Ativo,
                Personalizado = true
            };

            _context.Servicos.Add(novoServico);
            await _context.SaveChangesAsync();

            var agendamento = new Agendamentos
            {
                ClienteId = model.ClienteId,
                VeiculoId = veiculoIdFinal,
                ServicosId = novoServico.ServicosId,
                Data = model.Data,
                Estado = EstadoAgendamentos.Programada
            };

            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Clientes = _context.Clientes.ToList();
        ViewBag.Veiculos = _context.Veiculos.ToList();
        return View(model);
    }

}