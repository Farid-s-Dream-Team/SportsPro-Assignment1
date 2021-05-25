using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly SportsProContext _context;

        public RegistrationsController(SportsProContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var sportsProContext = _context.Customers.Include(c => c.Country);
            return View(await sportsProContext.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryID");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,FirstName,LastName,Address,City,State,PostalCode,CountryID,Phone,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryID", customer.CountryID);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryID", customer.CountryID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,FirstName,LastName,Address,City,State,PostalCode,CountryID,Phone,Email")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
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
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryID", customer.CountryID);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
        
        [HttpGet, ActionName("GetCustomer")]
        public IActionResult GetCustomer()
        {
            ViewBag.Customer = _context.Customers.ToList();  //changed Customers to Register
            var currentC = _context.Customers.Find(11);
            return View(currentC);
        }

        //List Customers Action
        //not 
        
        //make the form take a model of type customer
        [HttpGet]
        public async Task<IActionResult> List(int id) //try to read from 1-Data, 2-Parameters, 3-Query String
        {
            List<Registration> customers = null;
            if (id > 0)
            {
                customers = await _context.Registrations.Where(c => c.CustomerID == id).ToListAsync();
            }
            return View(customers);

        }

        [HttpPost]
        public async Task<IActionResult> List(Customer cust)    //this list gives admin option to delete currently assigined products
        {
            List<Registration> customers = null;
            if (cust.CustomerID > 0)
            {
                customers = await _context.Registrations.Where(c => c.CustomerID == cust.CustomerID).ToListAsync();
                return View(customers);
            }
            else //send a message to select a customer using Temp Data and redirect to Get Customer
                //insert TEMP DATA here (Must select a customer)
                return RedirectToAction("GetCustomer", "Registrations");
        }

    }
}
