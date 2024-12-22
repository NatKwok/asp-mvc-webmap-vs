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
using NetTopologySuite.Geometries;

namespace asp_mvc_webmap_vs.Controllers
{
    public class SignaleController : Controller
    {
        private readonly MvcWebmapContext _context;

        public SignaleController(MvcWebmapContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SignalementsCoyote>>> GetSignalementsCoyotes()
        {
            var feature = await _context.SignalementsCoyotes.Take(2).ToListAsync();

            //Map data to GeoJSON Features
            var features = feature.Select(record =>
            {

                // Check if Geom is not null
                if (record.Geom == null)
                {
                    // Handle cases where Geom is null (e.g., skip the record or provide a default value)
                    return null;
                }

                if (record.Geom is Point Point)

                {
                    // Replace with the actual fields for latitude and longitude in your model
                    var latitude = record.Geom.X;
                    var longitude = record.Geom.Y;

                    // Create a GeoJSON Point geometry
                    var point = new Point(latitude, longitude);

                    // Add additional properties from your model
                    var properties = new Dictionary<string, object>
                    {
                        { "Id", record.Id },
                        { "Date Observed", record.DatObs }, // Example field
                        { "Area", record.Territoire } // Example field
                    };

                    // Create a GeoJSON Feature
                    return new
                    {
                        type = "Feature",
                        geometry = point,
                        properties
                    };
                }

                return null;
            })
            .Where(feature => feature != null)
            .ToList();

            //Create a FeatureCollection
            var featureCollection = new
            {
                type = "FeatureCollection",
                features
            };
            //Serialize to GeoJSON
            var geoJson = JsonConvert.SerializeObject(featureCollection, Formatting.Indented,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    });

            // Return GeoJSON with appropriate content type
            return Content(geoJson, "application/json");
        }

        // GET: Signale
        public async Task<IActionResult> Index()
        {
            return View(await _context.SignalementsCoyotes.ToListAsync());
        }

        // GET: Signale/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signalementsCoyote = await _context.SignalementsCoyotes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (signalementsCoyote == null)
            {
                return NotFound();
            }

            return View(signalementsCoyote);
        }

        // GET: Signale/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Signale/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Geom,EntryId,ObjId,DatObs,HrObs,NbCoyotes,Alimentati,StatutAni,Periode,CompClass,ComCode,Cote,Territoire,StatutMen,Provenance,Verif,X,Y,Lat,Long")] SignalementsCoyote signalementsCoyote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(signalementsCoyote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(signalementsCoyote);
        }

        // GET: Signale/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signalementsCoyote = await _context.SignalementsCoyotes.FindAsync(id);
            if (signalementsCoyote == null)
            {
                return NotFound();
            }
            return View(signalementsCoyote);
        }

        // POST: Signale/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Geom,EntryId,ObjId,DatObs,HrObs,NbCoyotes,Alimentati,StatutAni,Periode,CompClass,ComCode,Cote,Territoire,StatutMen,Provenance,Verif,X,Y,Lat,Long")] SignalementsCoyote signalementsCoyote)
        {
            if (id != signalementsCoyote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(signalementsCoyote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SignalementsCoyoteExists(signalementsCoyote.Id))
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
            return View(signalementsCoyote);
        }

        // GET: Signale/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signalementsCoyote = await _context.SignalementsCoyotes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (signalementsCoyote == null)
            {
                return NotFound();
            }

            return View(signalementsCoyote);
        }

        // POST: Signale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var signalementsCoyote = await _context.SignalementsCoyotes.FindAsync(id);
            if (signalementsCoyote != null)
            {
                _context.SignalementsCoyotes.Remove(signalementsCoyote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SignalementsCoyoteExists(int id)
        {
            return _context.SignalementsCoyotes.Any(e => e.Id == id);
        }
    }
}
