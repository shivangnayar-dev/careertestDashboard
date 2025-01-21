using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auxx.Models;
using System.Data;

namespace Auxx.Controllers
{
    public class OrganizationReportsController : CommonController
    {
        //private readonly ApplicationDbContext _context;

        //public OrganizationReportsController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        public OrganizationReportsController(ApplicationDbContext context, IConfiguration configuration)
            : base(context, configuration) { }

        // GET: OrganizationReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrganizationReports.ToListAsync());
        }

        // GET: OrganizationReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationReports = await _context.OrganizationReports
                .FirstOrDefaultAsync(m => m.OrganizationReportId == id);
            if (organizationReports == null)
            {
                return NotFound();
            }

            return View(organizationReports);
        }

        // GET: OrganizationReports/Create
        public IActionResult Create()
        {
            ViewData["ReportsList"] = GetReportslist();
            var model = new OrganizationReports();

            
            return View(model);
        }

        public int? GetCost(string reportId)
        {
            var repordetail = _context.Reportdetails
                .FirstOrDefault(m => m.ReportId == reportId);

            return repordetail.Cost;
        }

        // POST: OrganizationReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationReports organizationReports)
        {
            // Remove Reportname validation
            ModelState.Remove("Reportname");
            // Remove Reportname validation
            ModelState.Remove("ReportDetail");
            if (ModelState.IsValid)
            {
                var selectedRole = organizationReports.ReportId;

                //organizationReports.CreatedBy = "shobha";//User.Identity.Name;
                //organizationReports.CreatedDate = DateTime.Now;
                _context.Add(organizationReports);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organizationReports);
        }

        // GET: OrganizationReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationReports = await _context.OrganizationReports.FindAsync(id);
            if (organizationReports == null)
            {
                return NotFound();
            }
            return View(organizationReports);
        }

        // POST: OrganizationReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrganizationReports organizationReports)
        {
            if (id != organizationReports.OrganizationReportId)
            {
                return NotFound();
            }
            // Remove Reportname validation
            ModelState.Remove("Reportname");
            // Remove Reportname validation
            ModelState.Remove("ReportDetail");
            if (ModelState.IsValid)
            {
                try
                {
                    //organizationReports.UpdatedBy = "shobha";//User.Identity.Name;
                    //organizationReports.UpdatedDate = DateTime.Now;
                    _context.Update(organizationReports);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationReportsExists(organizationReports.OrganizationReportId))
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
            return View(organizationReports);
        }

        // GET: OrganizationReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationReports = await _context.OrganizationReports
                .FirstOrDefaultAsync(m => m.OrganizationReportId == id);
            if (organizationReports == null)
            {
                return NotFound();
            }

            return View(organizationReports);
        }

        // POST: OrganizationReports/Delete/5
        [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organizationReports = await _context.OrganizationReports.FindAsync(id);
            if (organizationReports != null)
            {
                _context.OrganizationReports.Remove(organizationReports);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationReportsExists(int id)
        {
            return _context.OrganizationReports.Any(e => e.OrganizationReportId == id);
        }
    }
}
