using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptoManiaOficial.Contexto;
using LaptoManiaOficial.Models;
using Microsoft.AspNetCore.Authorization;

namespace LaptoManiaOficial.Controllers
{
    [Authorize]
    public class EquipoesController : Controller
    {
        private readonly MiContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EquipoesController(MiContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Equipoes
        public async Task<IActionResult> Index()
        {
              return _context.Equipos != null ? 
                          View(await _context.Equipos.ToListAsync()) :
                          Problem("Entity set 'MiContext.Equipos'  is null.");
        }

        // GET: Equipoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipo == null)
            {
                return NotFound();
            }

            return View(equipo);
        }

        // GET: Equipoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Marca,Modelo,Precio,Disponibilidad,Foto")] Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipo);
        }

        // GET: Equipoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return NotFound();
            }
            return View(equipo);
        }

        // POST: Equipoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Marca,Modelo,Precio,Disponibilidad,FotoFile")] Equipo equipo)
        {
            if (id != equipo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (equipo.FotoFile != null)
                    {
                        await SubirFoto(equipo);
                    }

                    _context.Update(equipo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipoExists(equipo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(equipo);
        }

        private async Task SubirFoto(Equipo equipo)
        {
            //formar el nombre de la foto
            string wwRootPath = _webHostEnvironment.WebRootPath;
            string extension = Path.GetExtension(equipo.FotoFile!.FileName);
            string nombreFoto = $"{equipo.Id}{extension}";

            equipo.Foto = nombreFoto;

            //copiar la foto en el proyecto del servidor
            string path = Path.Combine($"{wwRootPath}/fotos/equipos/", nombreFoto);
            var fileStream = new FileStream(path, FileMode.Create);
            await equipo.FotoFile.CopyToAsync(fileStream);
        }

        // GET: Equipoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipo = await _context.Equipos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipo == null)
            {
                return NotFound();
            }

            return View(equipo);
        }

        // POST: Equipoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Equipos == null)
            {
                return Problem("Entity set 'MiContext.Equipos'  is null.");
            }
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo != null)
            {
                _context.Equipos.Remove(equipo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipoExists(int id)
        {
          return (_context.Equipos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //PARA REPORTES
        public IActionResult ReporteEquiposDisponibles()
        {
            var equiposDisponibles = _context.Equipos.Where(e => e.Disponibilidad).ToList();
            return View(equiposDisponibles);
        }

        public IActionResult ReporteEquiposNoDisponibles()
        {
            var equiposNoDisponibles = _context.Equipos.Where(e => !e.Disponibilidad).ToList();
            return View(equiposNoDisponibles);
        }
    }
}
