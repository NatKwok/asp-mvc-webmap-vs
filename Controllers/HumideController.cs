using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using asp_mvc_webmap_vs.Data;
using asp_mvc_webmap_vs.Models;
using Newtonsoft.Json;

namespace asp_mvc_webmap_vs.Controllers
{
    public class HumideController : Controller
    {
        private readonly MvcWebmapContext _context;

        public HumideController(MvcWebmapContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MilieuxHumide>>> GetMilieuxHumides()
        {
            // Fetch data from the database
            var feature = await _context.MilieuxHumides.ToListAsync();

            // Map data to GeoJSON Features
            var features = feature.Select(record =>
            {
                if (record.Geom == null)
                    return null;

                if (record.Geom is NetTopologySuite.Geometries.MultiPolygon multiPolygon)
                {
                    // Extract coordinates for each polygon in the MultiPolygon
                    var multiPolygonCoordinates = multiPolygon.Geometries
                        .OfType<NetTopologySuite.Geometries.Polygon>()
                        .Select(polygon =>
                        {
                            // Extract the exterior ring
                            var exteriorRing = polygon.ExteriorRing.Coordinates
                                .Select(coord => new[] { coord.X, coord.Y }) // Flip to [latitude, longitude]
                                .ToList();

                            // Extract interior rings (holes)
                            var interiorRings = polygon.InteriorRings
                                .Select(ring => ring.Coordinates
                                .Select(coord => new[] { coord.X, coord.Y })
                                .ToList())
                                .ToList();

                            // Combine exterior and interior rings
                            var polygonCoordinates = new List<List<double[]>> { exteriorRing };
                            polygonCoordinates.AddRange(interiorRings);

                            return polygonCoordinates; // Return as List<List<Position>>
                        })
                        .ToList();

                    // Create a GeoJSON MultiPolygon geometry
                    var geoJsonMultiPolygon = new
                    {
                        type = "MultiPolygon",
                        coordinates = multiPolygonCoordinates
                    };
                    // Add additional properties from your model
                    var properties = new Dictionary<string, object>
                    {
                        { "Id", record.Id },
                        { "Type", record.MhId },
                        { "Class", record.ConsClDv }
                    };

                    // Create a GeoJSON Feature
                    return new
                    {
                        type = "Feature",
                        geometry = geoJsonMultiPolygon,
                        properties
                    };
                }

                return null; // Skip if not a MultiPolygon
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
