using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using asp_mvc_webmap_vs.Data;
using asp_mvc_webmap_vs.Models;
using Newtonsoft.Json;
using NetTopologySuite.Geometries;

namespace asp_mvc_webmap_vs.Controllers
{
    public class EcoterritoireController : Controller
    {
        private readonly MvcWebmapContext _context;

        public EcoterritoireController(MvcWebmapContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ecoterritoire>>> GetEcoterritoires()
        {
            var feature = await _context.Ecoterritoires.ToListAsync();
            var features = feature.Select(record =>
            {
                if (record.Geom == null)
                    return null;

                if (record.Geom is Polygon polygon)
                {
                    var polygonCoord = new List<List<Double[]>>()
                    {
                        polygon.ExteriorRing.Coordinates
                                .Select(coord => new[] { coord.X, coord.Y }) // Flip to [latitude, longitude]
                                .ToList()
                    };

                    var geoJsonPolygon = new
                    {
                        type = "Polygon",
                        coordinates = polygonCoord
                    };

                    // Add additional properties from your model
                    var properties = new Dictionary<string, object>
                    {
                        { "Id", record.Id },
                        { "Description", record.Text },
                        { "Area", record.ShapeArea }
                    };

                    // Create a GeoJSON Feature
                    return new
                    {
                        type = "Feature",
                        geometry = geoJsonPolygon,
                        properties
                    };
                }

                return null;
            })
                .Where(feature => feature != null) // Filter out null features
                .ToList();

            var featureCollection = new
            {
                type = "FeatureCollection",
                features
            };

            // Serialize to GeoJSON
            var geoJson = JsonConvert.SerializeObject(featureCollection);

            // Return GeoJSON with the appropriate content type
            return Content(geoJson, "application/json");
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
