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
    public class FunctionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FunctionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Functions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Functions.ToListAsync());
        }

        // GET: Functions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functions = await _context.Functions
                .FirstOrDefaultAsync(m => m.FunctionId == id);
            if (functions == null)
            {
                return NotFound();
            }

            return View(functions);
        }

        // GET: Functions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Functions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Functions functions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(functions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(functions);
        }

        // GET: Functions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functions = await _context.Functions.FindAsync(id);
            if (functions == null)
            {
                return NotFound();
            }
            return View(functions);
        }

        // POST: Functions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Functions functions)
        {
            if (id != functions.FunctionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(functions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionsExists(functions.FunctionId))
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
            return View(functions);
        }

        // GET: Functions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functions = await _context.Functions
                .FirstOrDefaultAsync(m => m.FunctionId == id);
            if (functions == null)
            {
                return NotFound();
            }

            return View(functions);
        }

        // POST: Functions/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var functions = await _context.Functions.FindAsync(id);
            if (functions != null)
            {
                _context.Functions.Remove(functions);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionsExists(int id)
        {
            return _context.Functions.Any(e => e.FunctionId == id);
        }
    }
}
