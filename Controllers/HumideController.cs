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
    public class HumideController : Controller
    {
        private readonly MvcWebmapContext _context;

        public HumideController(MvcWebmapContext context)
        {
            _context = context;
        }

        // GET: Humide
        public async Task<IActionResult> Index()
        {
            return View(await _context.MilieuxHumides.ToListAsync());
        }

        // GET: Humide/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milieuxHumide = await _context.MilieuxHumides
                .FirstOrDefaultAsync(m => m.Id == id);
            if (milieuxHumide == null)
            {
                return NotFound();
            }

            return View(milieuxHumide);
        }

        // GET: Humide/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Humide/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Geom,MhId,MhTypeDv,ConsClDv,Superficie")] MilieuxHumide milieuxHumide)
        {
            if (ModelState.IsValid)
            {
                _context.Add(milieuxHumide);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(milieuxHumide);
        }

        // GET: Humide/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milieuxHumide = await _context.MilieuxHumides.FindAsync(id);
            if (milieuxHumide == null)
            {
                return NotFound();
            }
            return View(milieuxHumide);
        }

        // POST: Humide/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Geom,MhId,MhTypeDv,ConsClDv,Superficie")] MilieuxHumide milieuxHumide)
        {
            if (id != milieuxHumide.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(milieuxHumide);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MilieuxHumideExists(milieuxHumide.Id))
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
            return View(milieuxHumide);
        }

        // GET: Humide/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milieuxHumide = await _context.MilieuxHumides
                .FirstOrDefaultAsync(m => m.Id == id);
            if (milieuxHumide == null)
            {
                return NotFound();
            }

            return View(milieuxHumide);
        }

        // POST: Humide/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var milieuxHumide = await _context.MilieuxHumides.FindAsync(id);
            if (milieuxHumide != null)
            {
                _context.MilieuxHumides.Remove(milieuxHumide);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MilieuxHumideExists(int id)
        {
            return _context.MilieuxHumides.Any(e => e.Id == id);
        }
    }
}
