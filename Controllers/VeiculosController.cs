using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Data;
using Proyecto_Programacion_III.Models.Entidades;
using System.Security.Claims;

public class VeiculosController : Controller
{
    private readonly Context _context;

    public VeiculosController(Context context)
    {
        _context = context;
    }

    private IQueryable<Cliente> ClientesDoUsuario()
    {
        var rol = User.FindFirstValue(ClaimTypes.Role);
        var usuarioIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var query = _context.Clientes.AsQueryable();

        if (rol == "Cliente" && usuarioIdStr != null)
        {
            int id = int.Parse(usuarioIdStr);
            query = query.Where(c => c.UsuarioId == id);
        }

        return query;
    }


    public async Task<IActionResult> Index()
    {
        var clienteIds = ClientesDoUsuario().Select(c => c.ClienteId);

        var veiculos = _context.Veiculos
            .Include(v => v.Cliente)
            .Where(v => clienteIds.Contains(v.ClienteId));

        return View(await veiculos.ToListAsync());
    }


    public async Task<IActionResult> Historico(int? id)
    {
        if (id == null) return NotFound();

        var clienteIds = ClientesDoUsuario().Select(c => c.ClienteId);

        var veiculo = await _context.Veiculos
            .Include(v => v.Cliente)
            .FirstOrDefaultAsync(v => v.VeiculoId == id && clienteIds.Contains(v.ClienteId));

        if (veiculo == null) return NotFound();

        var agendamentos = await _context.Agendamentos
            .Include(a => a.Servicos)
            .Where(a => a.VeiculoId == id)
            .OrderByDescending(a => a.Data)
            .ToListAsync();

        ViewBag.Veiculo = veiculo;

        return View(agendamentos);
    }


    public IActionResult Create()
    {
        ViewBag.Clientes = ClientesDoUsuario().ToList();
        return View();
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Veiculo veiculo)
    {
        var clientePertenceAoUsuario = await ClientesDoUsuario()
            .AnyAsync(c => c.ClienteId == veiculo.ClienteId);

        if (!clientePertenceAoUsuario)
        {
            ModelState.AddModelError("ClienteId", "Cliente inválido.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(veiculo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Clientes = ClientesDoUsuario().ToList();
        return View(veiculo);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var clienteIds = ClientesDoUsuario().Select(c => c.ClienteId);

        var veiculo = await _context.Veiculos
            .FirstOrDefaultAsync(v => v.VeiculoId == id && clienteIds.Contains(v.ClienteId));

        if (veiculo == null) return NotFound();

        ViewBag.Clientes = ClientesDoUsuario().ToList();
        return View(veiculo);
    }

  
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Veiculo veiculo)
    {
        if (id != veiculo.VeiculoId) return NotFound();

        var clientePertenceAoUsuario = await ClientesDoUsuario()
            .AnyAsync(c => c.ClienteId == veiculo.ClienteId);

        if (!clientePertenceAoUsuario)
        {
            ModelState.AddModelError("ClienteId", "Cliente inválido.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(veiculo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Veiculos.Any(v => v.VeiculoId == veiculo.VeiculoId))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Clientes = ClientesDoUsuario().ToList();
        return View(veiculo);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var clienteIds = ClientesDoUsuario().Select(c => c.ClienteId);

        var veiculo = await _context.Veiculos
            .Include(v => v.Cliente)
            .FirstOrDefaultAsync(v => v.VeiculoId == id && clienteIds.Contains(v.ClienteId));

        if (veiculo == null) return NotFound();

        return View(veiculo);
    }

 
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var clienteIds = ClientesDoUsuario().Select(c => c.ClienteId);

        var veiculo = await _context.Veiculos
            .FirstOrDefaultAsync(v => v.VeiculoId == id && clienteIds.Contains(v.ClienteId));

        if (veiculo != null)
        {
            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}