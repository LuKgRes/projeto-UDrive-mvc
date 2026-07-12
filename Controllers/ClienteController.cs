using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Data;
using Proyecto_Programacion_III.Models.Entidades;
using System.Security.Claims;

public class ClienteController : Controller
{
    private readonly Context _context;

    public ClienteController(Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var clientes = await _context.Clientes
            .Where(c => c.UsuarioId == usuarioId)
            .ToListAsync();

        return View(clientes);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Cliente cliente)
    {
        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }

        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        cliente.UsuarioId = usuarioId;

        if (!ModelState.IsValid)
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View(cliente);
        }

        if (ModelState.IsValid)
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(cliente);


    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Cliente cliente)
    {
        if (id != cliente.ClienteId)
            return NotFound();

        if (ModelState.IsValid)
        {
            bool existe = await _context.Clientes
                .AnyAsync(c => c.Identificacion == cliente.Identificacion
                               && c.ClienteId != cliente.ClienteId);

            if (existe)
            {
                ModelState.AddModelError("Identificacion", "Ya existe otro cliente con esta identificación.");
                return View(cliente);
            }

            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "No se pudo actualizar el cliente. Identificación duplicada.");
                return View(cliente);
            }

            return RedirectToAction(nameof(Index));
        }

        return View(cliente);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(m => m.ClienteId == id);

        if (cliente == null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente != null)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}

