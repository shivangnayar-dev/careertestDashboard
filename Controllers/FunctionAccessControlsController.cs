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
    public class FunctionAccessControlsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FunctionAccessControlsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FunctionAccessControls
        public async Task<IActionResult> Index()
        {
            return View(await _context.FunctionAccessControl.ToListAsync());
        }

        // GET: FunctionAccessControls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionAccessControl = await _context.FunctionAccessControl
                .FirstOrDefaultAsync(m => m.FunctionAccessId == id);
            if (functionAccessControl == null)
            {
                return NotFound();
            }

            return View(functionAccessControl);
        }

        // GET: FunctionAccessControls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FunctionAccessControls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FunctionAccessControl functionAccessControl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(functionAccessControl);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(functionAccessControl);
        }

        // GET: FunctionAccessControls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionAccessControl = await _context.FunctionAccessControl.FindAsync(id);
            if (functionAccessControl == null)
            {
                return NotFound();
            }
            return View(functionAccessControl);
        }

        // POST: FunctionAccessControls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FunctionAccessControl functionAccessControl)
        {
            if (id != functionAccessControl.FunctionAccessId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(functionAccessControl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionAccessControlExists(functionAccessControl.FunctionAccessId))
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
            return View(functionAccessControl);
        }

        // GET: FunctionAccessControls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functionAccessControl = await _context.FunctionAccessControl
                .FirstOrDefaultAsync(m => m.FunctionAccessId == id);
            if (functionAccessControl == null)
            {
                return NotFound();
            }

            return View(functionAccessControl);
        }

        // POST: FunctionAccessControls/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var functionAccessControl = await _context.FunctionAccessControl.FindAsync(id);
            if (functionAccessControl != null)
            {
                _context.FunctionAccessControl.Remove(functionAccessControl);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionAccessControlExists(int id)
        {
            return _context.FunctionAccessControl.Any(e => e.FunctionAccessId == id);
        }
    }
}
