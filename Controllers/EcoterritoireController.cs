using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using asp_mvc_webmap_vs.Data;
using asp_mvc_webmap_vs.Models;

namespace asp_mvc_webmap_vs.Controllers
{
    public class EcoterritoireController : Controller
    {
        private readonly MvcWebmapContext _context;

        public EcoterritoireController(MvcWebmapContext context)
        {
            _context = context;
        }

        // GET: Ecoterritoire/Map
        public async Task<IActionResult> Map()
        {
            return View(await _context.Ecoterritoires.ToListAsync());
        }

        // GET: Ecoterritoire
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ecoterritoires.ToListAsync());
        }

        // GET: Ecoterritoire/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ecoterritoire = await _context.Ecoterritoires
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ecoterritoire == null)
            {
                return NotFound();
            }

            return View(ecoterritoire);
        }

        // GET: Ecoterritoire/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ecoterritoire/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Geom,Text,ShapeLeng,ShapeArea")] Ecoterritoire ecoterritoire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ecoterritoire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ecoterritoire);
        }

        // GET: Ecoterritoire/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ecoterritoire = await _context.Ecoterritoires.FindAsync(id);
            if (ecoterritoire == null)
            {
                return NotFound();
            }
            return View(ecoterritoire);
        }

        // POST: Ecoterritoire/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Geom,Text,ShapeLeng,ShapeArea")] Ecoterritoire ecoterritoire)
        {
            if (id != ecoterritoire.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ecoterritoire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EcoterritoireExists(ecoterritoire.Id))
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
            return View(ecoterritoire);
        }

        // GET: Ecoterritoire/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ecoterritoire = await _context.Ecoterritoires
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ecoterritoire == null)
            {
                return NotFound();
            }

            return View(ecoterritoire);
        }

        // POST: Ecoterritoire/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ecoterritoire = await _context.Ecoterritoires.FindAsync(id);
            if (ecoterritoire != null)
            {
                _context.Ecoterritoires.Remove(ecoterritoire);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EcoterritoireExists(int id)
        {
            return _context.Ecoterritoires.Any(e => e.Id == id);
        }
    }
}
