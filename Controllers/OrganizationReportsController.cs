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
        public OrganizationReportsController(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor) { }

        // GET: OrganizationReports
        public async Task<IActionResult> Index()
        {
            var data = await (from or in _context.OrganizationReports
                              join org in _context.Organizations on or.OrganizationId equals org.OrganizationId
                              select new 
                              {
                                  OrganizationId = or.OrganizationId,
                                  ReportId = or.ReportId,
                                  ReportName = or.Reportname,
                                  Minimumcostofreport = or.Minimumcostofreport,
                                  MarkuponMinimumcost = or.MarkuponMinimumcost,
                                  TotalCost = or.TotalCost,
                                  Contract_Startdate = or.Contract_Startdate,
                                  Contract_Enddate = or.Contract_Enddate,
                                  CreatedDate = or.CreatedDate,
                                  CreatedBy = or.CreatedBy,
                                  UpdatedDate = or.UpdatedDate,
                                  UpdatedBy = or.UpdatedBy,
                                  OrganizationName = org != null ? org.OrganizationName : null,
                              }).ToListAsync();
            return View(data);
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
            ViewData["OrganizationsList"] = GetOrganizationslist();
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
            ModelState.Remove("OrganizationName");
            ModelState.Remove("Reportname");
            if (ModelState.IsValid)
            {
                _context.Add(organizationReports);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationsList"] = GetOrganizationslist();
            ViewData["ReportsList"] = GetReportslist();
            return View(organizationReports);
        }

        // GET: OrganizationReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["ReportsList"] = GetReportslist();
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
            ModelState.Remove("Reportname");
            if (ModelState.IsValid)
            {
                try
                {
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