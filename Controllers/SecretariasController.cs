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
    [Authorize(Roles = "Administrador")]
    public class SecretariasController : Controller
    {
        private readonly MiContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SecretariasController(MiContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Secretarias
        public async Task<IActionResult> Index()
        {
            var miContext = _context.Secretarias.Include(s => s.Usuario);
            return View(await miContext.ToListAsync());
        }

        // GET: Secretarias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Secretarias == null)
            {
                return NotFound();
            }

            var secretaria = await _context.Secretarias
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (secretaria == null)
            {
                return NotFound();
            }

            return View(secretaria);
        }

        // GET: Secretarias/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "CorreoElectronico");
            return View();
        }

        // POST: Secretarias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ci,NombreCompleto,Foto,UsuarioId")] Secretaria secretaria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(secretaria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "CorreoElectronico", secretaria.UsuarioId);
            return View(secretaria);
        }

        // GET: Secretarias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Secretarias == null)
            {
                return NotFound();
            }

            var secretaria = await _context.Secretarias.FindAsync(id);
            if (secretaria == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "CorreoElectronico", secretaria.UsuarioId);
            return View(secretaria);
        }

        // POST: Secretarias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ci,NombreCompleto,FotoFile,UsuarioId")] Secretaria secretaria)
        {
            if (id != secretaria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (secretaria.FotoFile != null)
                    {
                        await SubirFoto(secretaria);
                    }

                    _context.Update(secretaria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecretariaExists(secretaria.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "CorreoElectronico", secretaria.UsuarioId);
            return View(secretaria);
        }

        private async Task SubirFoto(Secretaria secretaria)
        {
            //formar el nombre de la foto
            string wwRootPath = _webHostEnvironment.WebRootPath;
            string extension = Path.GetExtension(secretaria.FotoFile!.FileName);
            string nombreFoto = $"{secretaria.Id}{extension}";

            secretaria.Foto = nombreFoto;

            //copiar la foto en el proyecto del servidor
            string path = Path.Combine($"{wwRootPath}/fotos/secretarias/", nombreFoto);
            var fileStream = new FileStream(path, FileMode.Create);
            await secretaria.FotoFile.CopyToAsync(fileStream);
        }

        // GET: Secretarias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Secretarias == null)
            {
                return NotFound();
            }

            var secretaria = await _context.Secretarias
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (secretaria == null)
            {
                return NotFound();
            }

            return View(secretaria);
        }

        // POST: Secretarias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Secretarias == null)
            {
                return Problem("Entity set 'MiContext.Secretarias'  is null.");
            }
            var secretaria = await _context.Secretarias.FindAsync(id);
            if (secretaria != null)
            {
                _context.Secretarias.Remove(secretaria);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecretariaExists(int id)
        {
          return (_context.Secretarias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
