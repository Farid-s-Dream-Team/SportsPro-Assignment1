//Author - Grant


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsPro.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Delete(int? ProductID, int? CustomerID)
        {
            if (CustomerID == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .FirstOrDefaultAsync(m => m.CustomerID == CustomerID && m.ProductID == ProductID);
            if (registration == null)
            {
                return NotFound();
            }
            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{ProductID} Removed from Customer";
            return RedirectToAction("List", new { CustomerID = CustomerID});
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
            //var currentC = _context.Customers.Find(11);
            //return View(currentC);
            return View();
        }

        //List Customers Action
        
        //make the form take a model of type customer
        [HttpGet]
        public async Task<IActionResult> List(int CustomerID) //try to read from 1-Data, 2-Parameters, 3-Query String
        {

            List<Registration> regists = _context.Registrations.Include(c => c.Customer)
                                       .Include(p => p.Product)
                                       .Where(r => r.CustomerID == CustomerID).ToList();

            CustomerViewModel register = new CustomerViewModel()
            {
                Registrations = regists,
                Products = _context.Products.ToList(),
                Customer = _context.Customers.Find(CustomerID),
                CustomerID = CustomerID
            };


            //List<Registration> customers = null;
            //if (CustomerID > 0)
            //{
            //    customers = await _context.Registrations.Where(c => c.CustomerID == CustomerID).ToListAsync();
            //}
            return View(register);

        }

        [HttpPost]
        public async Task<IActionResult> List(CustomerViewModel registervm)    //this list gives admin option to delete currently assigined products
        {

            if (registervm.ProductID == 0) // there is no product selected
            {
                TempData["Message"] = "Please select a product";
                return RedirectToAction("List", new { CustomerID = registervm.CustomerID });
            }

            var newreg = new Registration()
            {
                CustomerID = registervm.CustomerID,
                ProductID = registervm.ProductID
            };

            _context.Registrations.Add(newreg);
            _context.SaveChanges();
            TempData["Message"] = $" {registervm.ProductID} has been added to customer.";

            return RedirectToAction("List", new { CustomerID = registervm.CustomerID });




            //List<Registration> customers = null;
            //if (cust.CustomerID > 0)
            //{
            //    customers = await _context.Registrations.Where(c => c.CustomerID == cust.CustomerID).ToListAsync();
            //    return View(customers);
            //}
            //else //send a message to select a customer using Temp Data and redirect to Get Customer
            //    //insert TEMP DATA here (Must select a customer)
            //    TempData["registration"] = $"You must select a customer to proceed";
            //    return RedirectToAction("GetCustomer", "Registrations");
        }

    }
}
