using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auxx.Models;
using System.Configuration;

namespace Auxx.Controllers
{
    public class OrganzationLevelAccessesController : CommonController
    {
        //private readonly ApplicationDbContext _context;

        //public OrganzationLevelAccessesController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        private readonly string _connectionString;
        AuthController auth = null;

        public OrganzationLevelAccessesController(ApplicationDbContext context, IConfiguration configuration)
            : base(context, configuration) // Call the CommonController constructor
        {
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            auth = new AuthController(context, configuration);
        }

        // GET: OrganzationLevelAccesses
        public async Task<IActionResult> Index()
        {
            var data = await (from access in _context.OrganzationLevelAccess
                              join org in _context.Organizations
                              on access.OrganizationId equals org.OrganizationId
                              select new OrganzationLevelAccess
                              {
                                  OrgLevelAccesslId = access.OrgLevelAccesslId,
                                  Levels_Email6 = access.Levels_Email6,
                                  AccessEmail1 = access.AccessEmail1,
                                  AccessEmail2 = access.AccessEmail2,
                                  AccessEmail3 = access.AccessEmail3,
                                  AccessEmail4 = access.AccessEmail4,
                                  AccessEmail5 = access.AccessEmail5,
                                  AccessEmail6 = access.AccessEmail6,
                                  AccessEmail_Core = access.AccessEmail_Core,
                                  CreatedBy = access.CreatedBy,
                                  CreatedDate = access.CreatedDate,
                                  Enabled_Email6 = access.Enabled_Email6,
                                  Enddate_Email6 = access.Enddate_Email6,
                                  OrganizationId = access.OrganizationId,
                                  Startdate_Email6 = access.Startdate_Email6,
                                  Username_Email6 = access.Username_Email6,
                                  UpdatedBy = access.UpdatedBy,
                                  UpdatedDate = access.UpdatedDate,
                                  OrganizationName = org.OrganizationName
                              }).ToListAsync();

            return View(data);

            ///return View(await _context.OrganzationLevelAccess.ToListAsync());
        }

        // GET: OrganzationLevelAccesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organzationLevelAccess = await _context.OrganzationLevelAccess
                .FirstOrDefaultAsync(m => m.OrgLevelAccesslId == id);
            if (organzationLevelAccess == null)
            {
                return NotFound();
            }

            return View(organzationLevelAccess);
        }

        // GET: OrganzationLevelAccesses/Create
        public IActionResult Create()
        {
            ViewData["OrganizationsList"] = GetOrganizationslist();
            ViewData["LevelLists"] = GetAccesslist();
            var model = new OrganzationLevelAccess();
            model.Startdate_Core = DateTime.Now;
            model.Enddate_Core = DateTime.Now.AddDays(30);
            model.Startdate_Email1 = DateTime.Now;
            model.Enddate_Email1 = DateTime.Now.AddDays(30);
            model.Startdate_Email2 = DateTime.Now;
            model.Enddate_Email2 = DateTime.Now.AddDays(30);
            model.Startdate_Email3 = DateTime.Now;
            model.Enddate_Email3 = DateTime.Now.AddDays(30);
            model.Startdate_Email4 = DateTime.Now;
            model.Enddate_Email4 = DateTime.Now.AddDays(30);
            model.Startdate_Email5 = DateTime.Now;
            model.Enddate_Email5 = DateTime.Now.AddDays(30);
            model.Startdate_Email6 = DateTime.Now;
            model.Enddate_Email6 = DateTime.Now.AddDays(30);
            return View(model);
        }

        // POST: OrganzationLevelAccesses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganzationLevelAccess organzationLevelAccess)
        {
            // Remove Reportname validation
            ModelState.Remove("Organizations");
            // Remove Reportname validation
            ModelState.Remove("OrganizationName");

            if (ModelState.IsValid)
            {
                try
                {
                    //get Admin role id from roles table
                    var adminroleid = _context.Roles.Where(x => x.RoleName == "Admin").FirstOrDefault().RoleId;

                    //register Access email core 
                    LoginModel lmcore = new LoginModel();
                    lmcore.Email = organzationLevelAccess.AccessEmail_Core;
                    lmcore.Password = auth.HashPassword("Career@123#");
                    lmcore.Roleid = adminroleid;
                    lmcore.Organizationid = organzationLevelAccess.OrganizationId;
                    var coreresgister = auth.RegisterNewUser(lmcore);
                    if (coreresgister == 1)
                    {
                        ModelState.AddModelError("AccessEmail_Core", "The email is already exists.");
                    }

                    //register Access email1 
                    LoginModel lm1 = new LoginModel();
                    lm1.Email = organzationLevelAccess.AccessEmail1;
                    lm1.Password = auth.HashPassword("Career@123#");
                    lm1.Roleid = adminroleid;
                    lm1.Organizationid = organzationLevelAccess.OrganizationId;
                    var email1resgister = auth.RegisterNewUser(lm1);
                    if (email1resgister == 1)
                    {
                        ModelState.AddModelError("AccessEmail1", "The email is already exists.");
                    }
                    //register Access email2 
                    LoginModel lm2 = new LoginModel();
                    lm2.Email = organzationLevelAccess.AccessEmail2;
                    lm2.Password = auth.HashPassword("Career@123#");
                    lm2.Roleid = adminroleid;
                    lm2.Organizationid = organzationLevelAccess.OrganizationId;
                    var email2resgister = auth.RegisterNewUser(lm2);
                    if (email2resgister == 1)
                    {
                        ModelState.AddModelError("AccessEmail2", "The email is already exists.");
                    }
                    //register Access email3 
                    LoginModel lm3 = new LoginModel();
                    lm3.Email = organzationLevelAccess.AccessEmail3;
                    lm3.Password = auth.HashPassword("Career@123#");
                    lm3.Roleid = adminroleid;
                    lm3.Organizationid = organzationLevelAccess.OrganizationId;
                    var email3resgister = auth.RegisterNewUser(lm3);
                    if (email3resgister == 1)
                    {
                        ModelState.AddModelError("AccessEmail3", "The email is already exists.");
                    }

                    //register Access email4
                    LoginModel lm4 = new LoginModel();
                    lm4.Email = organzationLevelAccess.AccessEmail4;
                    lm4.Password = auth.HashPassword("Career@123#");
                    lm4.Roleid = adminroleid;
                    lm4.Organizationid = organzationLevelAccess.OrganizationId;
                    var email4resgister = auth.RegisterNewUser(lm4);
                    if (email4resgister == 1)
                    {
                        ModelState.AddModelError("AccessEmail4", "The email is already exists.");
                    }
                    //register Access email5
                    LoginModel lm5 = new LoginModel();
                    lm5.Email = organzationLevelAccess.AccessEmail5;
                    lm5.Password = auth.HashPassword("Career@123#");
                    lm5.Roleid = adminroleid;
                    lm5.Organizationid = organzationLevelAccess.OrganizationId;
                    var email5resgister = auth.RegisterNewUser(lm5);
                    if (email5resgister == 1)
                    {
                        ModelState.AddModelError("AccessEmail5", "The email is already exists.");
                    }
                    //register Access email6
                    LoginModel lm6 = new LoginModel();
                    lm6.Email = organzationLevelAccess.AccessEmail6;
                    lm6.Password = auth.HashPassword("Career@123#");
                    lm6.Roleid = adminroleid;
                    lm6.Organizationid = organzationLevelAccess.OrganizationId;
                    var email6resgister = auth.RegisterNewUser(lm6);
                    if (email6resgister == 1)
                    {
                        ModelState.AddModelError("AccessEmail6", "The email is already exists.");
                    }
                    if (email6resgister == 2)
                    {
                        _context.Add(organzationLevelAccess);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.CustomError = "The email is already exists.";
                    //ModelState.AddModelError("ModelOnly", "The email is already exists.");
                    ViewData["OrganizationsList"] = GetOrganizationslist();
                    ViewData["LevelLists"] = GetAccesslist();
                    return View(organzationLevelAccess);
                }

            }
            ViewData["OrganizationsList"] = GetOrganizationslist();
            ViewData["LevelLists"] = GetAccesslist();
            return View(organzationLevelAccess);
        }

        // GET: OrganzationLevelAccesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["OrganizationsList"] = GetOrganizationslist();
            ViewData["LevelLists"] = GetAccesslist();
            var organzationLevelAccess = await _context.OrganzationLevelAccess.FindAsync(id);
            if (organzationLevelAccess == null)
            {
                return NotFound();
            }
            return View(organzationLevelAccess);
        }

        // POST: OrganzationLevelAccesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrganzationLevelAccess organzationLevelAccess)
        {
            if (id != organzationLevelAccess.OrgLevelAccesslId)
            {
                return NotFound();
            }
            // Remove Reportname validation
            ModelState.Remove("Organizations");
            // Remove Reportname validation
            ModelState.Remove("OrganizationName");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organzationLevelAccess);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganzationLevelAccessExists(organzationLevelAccess.OrgLevelAccesslId))
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
            ViewData["OrganizationsList"] = GetOrganizationslist();
            ViewData["LevelLists"] = GetAccesslist();
            return View(organzationLevelAccess);
        }

        // GET: OrganzationLevelAccesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organzationLevelAccess = await _context.OrganzationLevelAccess
                .FirstOrDefaultAsync(m => m.OrgLevelAccesslId == id);
            if (organzationLevelAccess == null)
            {
                return NotFound();
            }

            return View(organzationLevelAccess);
        }

        // POST: OrganzationLevelAccesses/Delete/5
        [HttpPost, ActionName("Delete")]
        //        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organzationLevelAccess = await _context.OrganzationLevelAccess.FindAsync(id);
            if (organzationLevelAccess != null)
            {
                _context.OrganzationLevelAccess.Remove(organzationLevelAccess);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganzationLevelAccessExists(int id)
        {
            return _context.OrganzationLevelAccess.Any(e => e.OrgLevelAccesslId == id);
        }
    }
}