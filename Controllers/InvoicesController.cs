using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auxx.Models;

namespace Auxx.Controllers
{
    public class InvoicesController : CommonController
    {
        //private readonly string _connectionString;
       // AuthController auth = null;

        public InvoicesController(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor) // Call the CommonController constructor
        {
           // _connectionString = _configuration.GetConnectionString("DefaultConnection");
           // auth = new AuthController(context, configuration, httpContextAccessor);
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invoices.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = await _context.Invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoices == null)
            {
                return NotFound();
            }

            return View(invoices);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            Viewdatalists();



            return View();
        }

        private int GetTestCounts(int orgid)
        {
           // int organizationId = selectedOrganizationId; // Replace with actual selected OrganizationId

            var result = (from data in _context.cgstagingdata
                          join org in _context.Organizations
                          on data.organization equals org.OrganizationName
                          where org.OrganizationId == orgid
                          group data by org.OrganizationName into groupedData
                          select new
                          {
                              OrganizationName = groupedData.Key,
                              TestCodeCount = groupedData.Count(x => x.TestCode != null)
                          }).FirstOrDefault();


            return result.TestCodeCount;
        }

        private int GetReportCounts(int orgid)
        {
            // int organizationId = selectedOrganizationId; // Replace with actual selected OrganizationId

            var result = (from data in _context.cgstagingdata
                          join org in _context.Organizations
                          on data.organization equals org.OrganizationName
                          where org.OrganizationId == orgid
                          group data by org.OrganizationName into groupedData
                          select new
                          {
                              OrganizationName = groupedData.Key,
                              TestCodeCount = groupedData.Count(x => x.TestCode != null)
                          }).FirstOrDefault();


            return result.TestCodeCount;
        }
        private void Viewdatalists()
        {
            ViewData["OrganizationsList"] = GetOrganizationslist();
            ViewData["OrganizationReportsList"] = GetOrganizationReportslist();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Invoices invoices)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoices);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = await _context.Invoices.FindAsync(id);
            if (invoices == null)
            {
                return NotFound();
            }
            return View(invoices);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Invoices invoices)
        {
            if (id != invoices.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoicesExists(invoices.Id))
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
            return View(invoices);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoices = await _context.Invoices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoices == null)
            {
                return NotFound();
            }

            return View(invoices);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoices = await _context.Invoices.FindAsync(id);
            if (invoices != null)
            {
                _context.Invoices.Remove(invoices);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoicesExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
