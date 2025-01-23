using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auxx.Models;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Core.Types;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace Auxx.Controllers
{
    public class OrganizationLevelAccessesController : CommonController
    {
        AuthController auth = null;
        public OrganizationLevelAccessesController(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor) // Call the CommonController constructor
        {
            auth = new AuthController(context, configuration, httpContextAccessor);
        }

        // GET: OrganzationLevelAccesses
        public async Task<IActionResult> Index()
        {
            //return View(await _context.OrganizationLevelAccess.ToListAsync());

            var data = await (from access in _context.OrganizationLevelAccess
                              join org in _context.Organizations on access.OrganizationId equals org.OrganizationId into orgGroup
                              from org in orgGroup.DefaultIfEmpty() // Left join
                              join lc in _context.Levels on access.Levels_Core equals lc.LevelId.ToString() into levelsGroup
                              from lc in levelsGroup.DefaultIfEmpty() // Left join
                              join l1 in _context.Levels on access.Levels_Email1 equals l1.LevelId.ToString() into levelsGroup1
                              from l1 in levelsGroup1.DefaultIfEmpty() // Left join
                              join l2 in _context.Levels on access.Levels_Email2 equals l2.LevelId.ToString() into levelsGroup2
                              from l2 in levelsGroup2.DefaultIfEmpty() // Left join
                              join l3 in _context.Levels on access.Levels_Email3 equals l3.LevelId.ToString() into levelsGroup3
                              from l3 in levelsGroup3.DefaultIfEmpty() // Left join
                              join l4 in _context.Levels on access.Levels_Email4 equals l4.LevelId.ToString() into levelsGroup4
                              from l4 in levelsGroup4.DefaultIfEmpty() // Left join
                              join l5 in _context.Levels on access.Levels_Email5 equals l5.LevelId.ToString() into levelsGroup5
                              from l5 in levelsGroup5.DefaultIfEmpty() // Left join
                              join l6 in _context.Levels on access.Levels_Email6 equals l6.LevelId.ToString() into levelsGroup6
                              from l6 in levelsGroup6.DefaultIfEmpty() // Left join
                              select new OrganizationAccessViewModel
                              {
                                  OrganizationLevelAccess = access,
                                  OrganizationName = org != null ? org.OrganizationName : null,
                                  Levelcore = lc != null ? lc.LevelName : "No Level Assigned",
                                  Levelname1 = l1 != null ? l1.LevelName : "No Level Assigned",
                                  Levelname2 = l2 != null ? l2.LevelName : "No Level Assigned",
                                  Levelname3 = l3 != null ? l3.LevelName : "No Level Assigned",
                                  Levelname4 = l4 != null ? l4.LevelName : "No Level Assigned",
                                  Levelname5 = l5 != null ? l5.LevelName : "No Level Assigned",
                                  Levelname6 = l6 != null ? l6.LevelName : "No Level Assigned",
                                  // Repeat for other Levels_Email fields as needed
                              }).ToListAsync();

            return View(data);

            /* var data = await (from access in _context.OrganizationLevelAccess
                               join org in _context.Organizations
                               on access.OrganizationId equals org.OrganizationId
                               select new OrganizationLevelAccess
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

             return View(data);*/

            ///return View(await _context.OrganzationLevelAccess.ToListAsync());
        }

        // GET: OrganizationLevelAccesses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationLevelAccess = await _context.OrganizationLevelAccess
                .FirstOrDefaultAsync(m => m.OrgLevelAccesslId == id);
            if (organizationLevelAccess == null)
            {
                return NotFound();
            }

            return View(organizationLevelAccess);
        }

        // GET: OrganzationLevelAccesses/Create
        public IActionResult Create()
        {
            Viewdatalists();
            var model = new OrganizationLevelAccess();
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
        public async Task<IActionResult> Create(OrganizationLevelAccess organizationLevelAccess)
        {
            ModelState.Remove("Organizations");
            ModelState.Remove("OrganizationName");

            if (ModelState.IsValid)
            {
                try
                {
                    //TODO - check with subhashini if all user should be superadmin or should consider as admin role
                    //get Admin role id from roles table
                    var adminroleid = _context.Roles.Where(x => x.RoleName == "SuperAdmin").FirstOrDefault().RoleId;

                    #region Email Logins - add all email detail to user table with default password. If existing user detail then break next execution
                    //register Access email core 
                    if (organizationLevelAccess.AccessEmail_Core != string.Empty)
                    {
                        User lmcore = new User();
                        lmcore.Email = organizationLevelAccess.AccessEmail_Core;
                        lmcore.PasswordHash = auth.HashPassword("Testup@123");
                        lmcore.Roleid = adminroleid;
                        lmcore.Organizationid = organizationLevelAccess.OrganizationId;
                        lmcore.Level = _context.Levels.Where(x => x.LevelName == Convert.ToString(organizationLevelAccess.Levels_Core)).SingleOrDefaultAsync().Result.LevelId;
                        organizationLevelAccess.Levels_Core = lmcore.Level.ToString();
                        lmcore.CreatedDate = DateTime.Now;
                        lmcore.CreatedBy = _username;
                        var coreresgister = auth.RegisterNewUser(lmcore);
                        if (coreresgister == 1)
                        {
                            //ModelState.AddModelError("AccessEmail_Core", "The email is already exists.");
                            return HandleModelError("AccessEmail_Core", "The email already exists.", organizationLevelAccess);

                        }
                    }
                    //register Access email1 
                    if (organizationLevelAccess.AccessEmail1 != string.Empty)
                    {
                        User lm1 = new User();
                        lm1.Email = organizationLevelAccess.AccessEmail1;
                        lm1.PasswordHash = auth.HashPassword("Testup@123");
                        lm1.Roleid = adminroleid;
                        lm1.Organizationid = organizationLevelAccess.OrganizationId;
                        lm1.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email1);
                        lm1.CreatedDate = DateTime.Now;
                        lm1.CreatedBy = _username;
                        //_context.Levels.Where(x => x.LevelId == Convert.ToInt32(organizationLevelAccess.Levels_Email1)).SingleOrDefaultAsync().Result.LevelId;
                        var email1resgister = auth.RegisterNewUser(lm1);
                        if (email1resgister == 1)
                        {
                            return HandleModelError("AccessEmail1", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email2 
                    if (organizationLevelAccess.AccessEmail2 != string.Empty)
                    {
                        User lm2 = new User();
                        lm2.Email = organizationLevelAccess.AccessEmail2;
                        lm2.PasswordHash = auth.HashPassword("Testup@123");
                        lm2.Roleid = adminroleid;
                        lm2.Organizationid = organizationLevelAccess.OrganizationId;
                        lm2.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email2);
                        lm2.CreatedDate = DateTime.Now;
                        lm2.CreatedBy = _username;
                        //_context.Levels.Where(x => x.LevelName == organizationLevelAccess.Levels_Email2).SingleOrDefaultAsync().Result.LevelId;
                        var email2resgister = auth.RegisterNewUser(lm2);
                        if (email2resgister == 1)
                        {
                            return HandleModelError("AccessEmail2", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email3 
                    if (organizationLevelAccess.AccessEmail3 != string.Empty)
                    {
                        User lm3 = new User();
                        lm3.Email = organizationLevelAccess.AccessEmail3;
                        lm3.PasswordHash = auth.HashPassword("Testup@123");
                        lm3.Roleid = adminroleid;
                        lm3.Organizationid = organizationLevelAccess.OrganizationId;
                        lm3.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email3);
                        lm3.CreatedDate = DateTime.Now;
                        lm3.CreatedBy = _username;
                        //_context.Levels.Where(x => x.LevelName == organizationLevelAccess.Levels_Email3).SingleOrDefaultAsync().Result.LevelId;
                        var email3resgister = auth.RegisterNewUser(lm3);
                        if (email3resgister == 1)
                        {
                            return HandleModelError("AccessEmail3", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email4
                    if (organizationLevelAccess.AccessEmail4 != string.Empty)
                    {
                        User lm4 = new User();
                        lm4.Email = organizationLevelAccess.AccessEmail4;
                        lm4.PasswordHash = auth.HashPassword("Testup@123");
                        lm4.Roleid = adminroleid;
                        lm4.Organizationid = organizationLevelAccess.OrganizationId;
                        lm4.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email4);
                        lm4.CreatedDate = DateTime.Now;
                        lm4.CreatedBy = _username;
                        //_context.Levels.Where(x => x.LevelName == organizationLevelAccess.Levels_Email4).SingleOrDefaultAsync().Result.LevelId;
                        var email4resgister = auth.RegisterNewUser(lm4);
                        if (email4resgister == 1)
                        {
                            return HandleModelError("AccessEmail4", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email5
                    if (organizationLevelAccess.AccessEmail5 != string.Empty)
                    {
                        User lm5 = new User();
                        lm5.Email = organizationLevelAccess.AccessEmail5;
                        lm5.PasswordHash = auth.HashPassword("Testup@123");
                        lm5.Roleid = adminroleid;
                        lm5.Organizationid = organizationLevelAccess.OrganizationId;
                        lm5.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email5);
                        lm5.CreatedDate = DateTime.Now;
                        lm5.CreatedBy = _username;
                        //_context.Levels.Where(x => x.LevelName == organizationLevelAccess.Levels_Email5).SingleOrDefaultAsync().Result.LevelId;
                        var email5resgister = auth.RegisterNewUser(lm5);
                        if (email5resgister == 1)
                        {
                            return HandleModelError("AccessEmail5", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email6
                    if (organizationLevelAccess.AccessEmail6 != string.Empty)
                    {
                        User lm6 = new User();
                        lm6.Email = organizationLevelAccess.AccessEmail6;
                        lm6.PasswordHash = auth.HashPassword("Testup@123");
                        lm6.Roleid = adminroleid;
                        lm6.Organizationid = organizationLevelAccess.OrganizationId;
                        lm6.Level = _context.Levels.Where(x => x.LevelName == Convert.ToString(organizationLevelAccess.Levels_Email6)).SingleOrDefaultAsync().Result.LevelId;
                        lm6.CreatedDate = DateTime.Now;
                        lm6.CreatedBy = _username;
                        organizationLevelAccess.Levels_Email6 = lm6.Level.ToString();
                        var email6resgister = auth.RegisterNewUser(lm6);
                        if (email6resgister == 1)
                        {
                            return HandleModelError("AccessEmail6", "The email is already exists.", organizationLevelAccess);
                        }
                        if (email6resgister == 2)
                        {
                            _context.Add(organizationLevelAccess);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.CustomError = "The email is already exists.";
                    return HandleModelError("", "", organizationLevelAccess);
                }
                return RedirectToAction(nameof(Index));
            }
            return HandleModelError("", "", organizationLevelAccess);
        }

        private void Viewdatalists()
        {
            ViewData["OrganizationsList"] = GetOrganizationslist();
            if (_level == "Level1")
            {
                ViewData["LevelLists"] = GetLevellist().Where(x => x.Text != "Level1");
            }
            else if (_level == "Level2")
            {
                ViewData["LevelLists"] = GetLevellist().Where(x => x.Text != "Level2" && x.Text != "Level1");
            }
            else if (_level == "Level3")
            {
                ViewData["LevelLists"] = GetLevellist().Where(x => x.Text != "Level1" && x.Text != "Level2");
            }
        }

        private IActionResult HandleModelError(string propertyName, string errorMessage, OrganizationLevelAccess organizationLevelAccess)
        {
            if (propertyName != string.Empty)
            {
                ModelState.AddModelError(propertyName, errorMessage);
            }
            Viewdatalists(); // Ensure this sets any necessary ViewData
            return View(organizationLevelAccess);
        }


        // GET: OrganzationLevelAccesses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["OrganizationsList"] = GetOrganizationslist();
            ViewData["LevelLists"] = GetLevellist();
            var organizationLevelAccess = await _context.OrganizationLevelAccess.FindAsync(id);
            if (organizationLevelAccess == null)
            {
                return NotFound();
            }
            return View(organizationLevelAccess);
        }

        // POST: OrganzationLevelAccesses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrganizationLevelAccess organizationLevelAccess)
        {
            if (id != organizationLevelAccess.OrgLevelAccesslId)
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
                    var adminroleid = _context.Roles.Where(x => x.RoleName == "SuperAdmin").FirstOrDefault().RoleId;
                    organizationLevelAccess.Levels_Core = _context.Levels.Where(x => x.LevelName == Convert.ToString(organizationLevelAccess.Levels_Core)).SingleOrDefaultAsync().Result.LevelId.ToString();
                    organizationLevelAccess.Levels_Email6 = _context.Levels.Where(x => x.LevelName == Convert.ToString(organizationLevelAccess.Levels_Email6)).SingleOrDefaultAsync().Result.LevelId.ToString();

                    #region Email Logins - add all email detail to user etail with default password. If exist user detail then break next execution
                    //register Access email core 
                    if (organizationLevelAccess.AccessEmail_Core != string.Empty)
                    {
                        User lmcore = new User();
                        lmcore.Email = organizationLevelAccess.AccessEmail_Core;
                        lmcore.PasswordHash = auth.HashPassword("Testup@123");
                        lmcore.Roleid = adminroleid;
                        lmcore.Organizationid = organizationLevelAccess.OrganizationId;
                        lmcore.Level = Convert.ToInt32(organizationLevelAccess.Levels_Core);
                        lmcore.CreatedDate = DateTime.Now;
                        lmcore.CreatedBy = _username;
                        var coreresgister = auth.RegisterNewUser(lmcore);
                        if (coreresgister == 1)
                        {
                            //ModelState.AddModelError("AccessEmail_Core", "The email is already exists.");
                            return HandleModelError("AccessEmail_Core", "The email already exists.", organizationLevelAccess);

                        }
                    }
                    //register Access email1 
                    if (organizationLevelAccess.AccessEmail1 != string.Empty)
                    {
                        User lm1 = new User();
                        lm1.Email = organizationLevelAccess.AccessEmail1;
                        lm1.PasswordHash = auth.HashPassword("Testup@123");
                        lm1.Roleid = adminroleid;
                        lm1.Organizationid = organizationLevelAccess.OrganizationId;
                        lm1.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email1);
                        lm1.CreatedDate = DateTime.Now;
                        lm1.CreatedBy = _username;
                        var email1resgister = auth.RegisterNewUser(lm1);
                        if (email1resgister == 1)
                        {
                            //ModelState.AddModelError("AccessEmail1", "The email is already exists.");
                            return HandleModelError("AccessEmail1", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email2 
                    if (organizationLevelAccess.AccessEmail2 != string.Empty)
                    {
                        User lm2 = new User();
                        lm2.Email = organizationLevelAccess.AccessEmail2;
                        lm2.PasswordHash = auth.HashPassword("Testup@123");
                        lm2.Roleid = adminroleid;
                        lm2.Organizationid = organizationLevelAccess.OrganizationId;
                        lm2.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email2);
                        lm2.CreatedDate = DateTime.Now;
                        lm2.CreatedBy = _username;
                        var email2resgister = auth.RegisterNewUser(lm2);
                        if (email2resgister == 1)
                        {
                            //ModelState.AddModelError("AccessEmail2", "The email is already exists.");
                            return HandleModelError("AccessEmail2", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email3 
                    if (organizationLevelAccess.AccessEmail3 != string.Empty)
                    {
                        User lm3 = new User();
                        lm3.Email = organizationLevelAccess.AccessEmail3;
                        lm3.PasswordHash = auth.HashPassword("Testup@123");
                        lm3.Roleid = adminroleid;
                        lm3.Organizationid = organizationLevelAccess.OrganizationId;
                        lm3.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email3);
                        lm3.CreatedDate = DateTime.Now;
                        lm3.CreatedBy = _username;
                        var email3resgister = auth.RegisterNewUser(lm3);
                        if (email3resgister == 1)
                        {
                            //ModelState.AddModelError("AccessEmail3", "The email is already exists.");
                            return HandleModelError("AccessEmail3", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email4
                    if (organizationLevelAccess.AccessEmail4 != string.Empty)
                    {
                        User lm4 = new User();
                        lm4.Email = organizationLevelAccess.AccessEmail4;
                        lm4.PasswordHash = auth.HashPassword("Testup@123");
                        lm4.Roleid = adminroleid;
                        lm4.Organizationid = organizationLevelAccess.OrganizationId;
                        lm4.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email4);
                        lm4.CreatedDate = DateTime.Now;
                        lm4.CreatedBy = _username;
                        var email4resgister = auth.RegisterNewUser(lm4);
                        if (email4resgister == 1)
                        {
                            ModelState.AddModelError("AccessEmail4", "The email is already exists.");
                            return HandleModelError("AccessEmail4", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email5
                    if (organizationLevelAccess.AccessEmail5 != string.Empty)
                    {
                        User lm5 = new User();
                        lm5.Email = organizationLevelAccess.AccessEmail5;
                        lm5.PasswordHash = auth.HashPassword("Testup@123");
                        lm5.Roleid = adminroleid;
                        lm5.Organizationid = organizationLevelAccess.OrganizationId;
                        lm5.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email5);
                        lm5.CreatedDate = DateTime.Now;
                        lm5.CreatedBy = _username;
                        var email5resgister = auth.RegisterNewUser(lm5);
                        if (email5resgister == 1)
                        {
                            ModelState.AddModelError("AccessEmail5", "The email is already exists.");
                            return HandleModelError("AccessEmail5", "The email is already exists.", organizationLevelAccess);
                        }
                    }
                    //register Access email6
                    if (organizationLevelAccess.AccessEmail6 != string.Empty)
                    {
                        User lm6 = new User();
                        lm6.Email = organizationLevelAccess.AccessEmail6;
                        lm6.PasswordHash = auth.HashPassword("Testup@123");
                        lm6.Roleid = adminroleid;
                        lm6.Organizationid = organizationLevelAccess.OrganizationId;
                        lm6.Level = Convert.ToInt32(organizationLevelAccess.Levels_Email6);
                        lm6.CreatedDate = DateTime.Now;
                        lm6.CreatedBy = _username;
                        organizationLevelAccess.Levels_Email6 = lm6.Level.ToString();
                        var email6resgister = auth.RegisterNewUser(lm6);
                        if (email6resgister == 1)
                        {
                            ModelState.AddModelError("AccessEmail6", "The email is already exists.");
                            return HandleModelError("AccessEmail6", "The email is already exists.", organizationLevelAccess);
                        }
                        if (email6resgister == 2)
                        {
                            _context.Add(organizationLevelAccess);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    #endregion
                    _context.Update(organizationLevelAccess);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationLevelAccessExists(organizationLevelAccess.OrgLevelAccesslId))
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
            ViewData["LevelLists"] = GetLevellist();
            return View(organizationLevelAccess);
        }


        // GET: OrganzationLevelAccesses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationLevelAccess = await _context.OrganizationLevelAccess
                .FirstOrDefaultAsync(m => m.OrgLevelAccesslId == id);
            if (organizationLevelAccess == null)
            {
                return NotFound();
            }

            return View(organizationLevelAccess);
        }

        // POST: OrganzationLevelAccesses/Delete/5
        [HttpPost, ActionName("Delete")]
        //        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organizationLevelAccess = await _context.OrganizationLevelAccess.FindAsync(id);
            if (organizationLevelAccess != null)
            {
                _context.OrganizationLevelAccess.Remove(organizationLevelAccess);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationLevelAccessExists(int id)
        {
            return _context.OrganizationLevelAccess.Any(e => e.OrgLevelAccesslId == id);
        }
    }
}