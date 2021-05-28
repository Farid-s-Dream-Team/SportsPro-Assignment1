//Author - Group Work, Grant Farid Aaron & Srini


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    [Authorize]
    public class TechIncidentController : Controller
    {
        private readonly SportsProContext _context;

        public TechIncidentController(SportsProContext context)
        {
            _context = context;
        }

        // GET: LIst of Incidents by TechniciansID

        
        public async Task<IActionResult> List(int id)
        {
            ViewBag.Technician = _context.Technicians.Find(id);
            List<Incident> incidents = null;
            if (id > 0)
            {
                incidents = await _context.Incidents.Where(i => i.TechnicianID == id).ToListAsync();
            } 
            return View(incidents);
        }

        [HttpPost]
        public async Task<IActionResult> List(Technician tech)
        {
            List<Incident> incidents = null;
            if (tech.TechnicianID > 0)
            {
                incidents = await _context.Incidents.Where(i => i.TechnicianID == tech.TechnicianID).ToListAsync();
            }
            return View(incidents);
        }

        // GET: Technicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = await _context.Technicians
                .FirstOrDefaultAsync(m => m.TechnicianID == id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // GET: Technicians/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Technicians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TechnicianID,Name,Email,Phone")] Technician technician)
        {
            if (ModelState.IsValid)
            {
                _context.Add(technician);
                await _context.SaveChangesAsync();
                TempData["message"] = $"{technician.Name} has been created";
                return RedirectToAction(nameof(Index));
            }
            return View(technician);
        }

        // GET: Technicians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id); //changed from Technicians to Incidents
            if (incident == null)
            {
                return NotFound();
            }
            return View(incident);
        }

        // POST: Technicians/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TechnicianID,Name,Email,Phone")] Technician technician)
        {
            if (id != technician.TechnicianID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(technician);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechnicianExists(technician.TechnicianID))
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
            return View(technician);
        }

        // GET: Technicians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = await _context.Technicians
                .FirstOrDefaultAsync(m => m.TechnicianID == id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // POST: Technicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var technician = await _context.Technicians.FindAsync(id);
            _context.Technicians.Remove(technician);
            await _context.SaveChangesAsync();
            TempData["message"] = $"{technician.Name} is now deleted";
            return RedirectToAction(nameof(Index));
        }

        private bool TechnicianExists(int id)
        {
            return _context.Technicians.Any(e => e.TechnicianID == id);
        }

        [HttpGet, ActionName("AddEdit")]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "AddEdit";
            //ViewBag.Incidents = _context.Incidents.OrderBy(g => g.IncidentID).ToList();
            var tech = _context.Technicians.Find(id);
            return View(tech);
        }

        [HttpPost, ActionName("AddEdit")]
        public IActionResult Edit(Technician tech)
        {
            if (ModelState.IsValid)
            {
                if (tech.TechnicianID == 0)
                    _context.Technicians.Add(tech);
                else
                    _context.Technicians.Update(tech);
                _context.SaveChanges();
                TempData["message"] = $"{tech.Name} is now up to date";
                return RedirectToAction("Index", "Technicians");
            }
            else
            {
                ViewBag.Action = (tech.TechnicianID == 0) ? "Add" : "Edit";
                ViewBag.Incedents = _context.Incidents.OrderBy(g => g.IncidentID).ToList();
                return View(tech);
            }
        }

        [HttpGet, ActionName("Get")]
        public IActionResult Get()
        {
            ViewBag.Tech = _context.Technicians.ToList();
            var currentTech = _context.Technicians.Find(11);
            return View(currentTech);
        }






    }
}
