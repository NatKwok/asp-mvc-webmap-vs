using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using asp_mvc_webmap_vs.Data;
using asp_mvc_webmap_vs.Models;
using Newtonsoft.Json;
using GeoJSON.Text.Geometry;
using NetTopologySuite.IO;

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
        //public ContentResult GetEcoterritoireData()
        //{
        //    // Fetch GeoJSON FeatureCollection
        //    FeatureCollection featureCollection = _context.FetchGeoJsonFromDatabase();

        //    // Serialize FeatureCollection to JSON
        //    string geoJson = JsonSerializer.Serialize(featureCollection);

        //    // Return GeoJSON
        //    return Content(geoJson, "application/json");
        //}

        // GET: Ecoterritoire/Map
        public async Task<ActionResult> Map()
        {
            var feature = await _context.Ecoterritoires.ToListAsync();
            var geoJsonWriter = new GeoJsonWriter();
            var features = feature.Select(record =>
            {

                if (record.Geom == null)
                {
                    return null;
                }
                // Convert the geometry to a GeoJSON string
                var geometryJson = geoJsonWriter.Write(record.Geom);
                var geometry = JsonConvert.DeserializeObject<GeoJSON.Text.Geometry.IGeometryObject>(geometryJson);

                //var polygon = record.Geom.
                //    .Select(lineString => new LineString(
                //        lineString.Coordinates.Select(coord => new GeoJSON.Text.Geometry.Position(coord.Y, coord.X)).ToList()
                //    )).ToList();

                var properties = new Dictionary<string, object>
                        {
                            { "id", record.Id },
                            { "name", record.Text },
                            { "shapeLeng", record.ShapeLeng },
                            { "shapeArea", record.ShapeArea }
                        };

                return new GeoJSON.Text.Feature.Feature(geometry, properties);

            }).ToList();

            var featureCollection = GeojsonConverter.CreateFeatureCollection(features);
            var geoJson = JsonConvert.SerializeObject(featureCollection);
   
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
