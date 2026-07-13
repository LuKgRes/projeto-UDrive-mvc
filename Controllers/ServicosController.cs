using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programacion_III.Data;
using Proyecto_Programacion_III.Models.Entidades;
using Proyecto_Programacion_III.Models.Entidades.Opciones;

public class ServicosController : Controller
{
    private readonly Context _context;

    public ServicosController(Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var Servicos = await _context.Servicos
         .Where(s => !s.Personalizado)
         .ToListAsync();

        return View(Servicos);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Servicos Servicos)
    {
        Servicos.Estado = EstadoServicos.Ativo;

        if (ModelState.IsValid)
        {
            _context.Add(Servicos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        return View(Servicos);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var Servicos = await _context.Servicos.FindAsync(id);

        if (Servicos == null)
            return NotFound();

        return View(Servicos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Servicos Servicos)
    {
        if (id != Servicos.ServicosId)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(Servicos);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Servicos.Any(e => e.ServicosId == Servicos.ServicosId))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(Servicos);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var Servicos = await _context.Servicos
            .FirstOrDefaultAsync(m => m.ServicosId == id);

        if (Servicos == null)
            return NotFound();

        return View(Servicos);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var Servicos = await _context.Servicos.FindAsync(id);
        if (Servicos != null)
        {
            _context.Servicos.Remove(Servicos);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
