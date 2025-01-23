using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Auxx.Models;
using System.Data;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Auxx.Controllers
{
    public class OrganizationsController : CommonController
    {

       // private readonly string _connectionString;
        AuthController auth = null;

        public OrganizationsController(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor) // Call the CommonController constructor
        {
            //_connectionString = _configuration.GetConnectionString("DefaultConnection");
            auth = new AuthController(context, configuration, httpContextAccessor);
        }

        // GET: Organizations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Organizations.Where(x=>x.CreatedBy == User.Identity.Name).ToListAsync());
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizations = await _context.Organizations
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organizations == null)
            {
                return NotFound();
            }

            return View(organizations);
        }

        // GET: Organizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Organizations organizations)
        {
            organizations.CreatedBy = User.Identity.Name;
            organizations.CreatedDate = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(organizations);
                    await _context.SaveChangesAsync();

                    var level1id = _context.Levels.Where(x => x.LevelName == "Level 1" || x.LevelName == "Level1").FirstOrDefaultAsync().Result.LevelId;

                    #region logindetail
                    //Admin email id of carrergraph
                    if (organizations.CG_SuperAdminEmailId != string.Empty)
                    {
                        User cgemail = new User();
                        cgemail.Email = organizations.CG_SuperAdminEmailId;
                        cgemail.PasswordHash = auth.HashPassword("Testup@123");
                        cgemail.Roleid = _context.Roles.Where(x => x.RoleName == "CG Superadmin" || x.RoleName == "CGSuperadmin").FirstOrDefaultAsync().Result.RoleId;
                        cgemail.Organizationid = organizations.OrganizationId;
                        cgemail.Level = level1id;
                        cgemail.CreatedDate = DateTime.Now;
                        cgemail.CreatedBy = _username;
                        var email1resgister = auth.RegisterNewUser(cgemail);
                        if (email1resgister == 1)
                        {
                            ModelState.AddModelError("Customer_AdminEmailId1", "The email is already exists.");
                        }
                    }

                    //Admin email id for customer
                    if (organizations.Customer_SuperAdminEmailId != string.Empty)
                    {
                        User customeremail = new User();
                        customeremail.Email = organizations.Customer_SuperAdminEmailId;
                        customeremail.PasswordHash = auth.HashPassword("Testup@123");
                        customeremail.Roleid = _context.Roles.Where(x => x.RoleName == "SuperAdmin" || x.RoleName == "Superadmin").FirstOrDefaultAsync().Result.RoleId;
                        customeremail.Organizationid = organizations.OrganizationId;
                        customeremail.Level = level1id;
                        customeremail.CreatedDate = DateTime.Now;
                        customeremail.CreatedBy = _username;
                        var email2resgister = auth.RegisterNewUser(customeremail);
                        if (email2resgister == 1)
                        {
                            ModelState.AddModelError("Customer_AdminEmailId2", "The email is already exists.");
                        }
                    }

                    //Admin email id 1
                    if (organizations.Customer_AdminEmailId1 != string.Empty)
                    {
                        User lmemail1 = new User();
                        lmemail1.Email = organizations.Customer_AdminEmailId1;
                        lmemail1.PasswordHash = auth.HashPassword("Testup@123");
                        lmemail1.Roleid = _context.Roles.Where(x => x.RoleName == "Admin" || x.RoleName == "admin").FirstOrDefaultAsync().Result.RoleId; ;
                        lmemail1.Organizationid = organizations.OrganizationId;
                        lmemail1.Level = level1id;
                        lmemail1.CreatedDate = DateTime.Now;
                        lmemail1.CreatedBy = _username;
                        var email3resgister = auth.RegisterNewUser(lmemail1);
                        if (email3resgister == 1)
                        {
                            ModelState.AddModelError("Customer_AdminEmailId3", "The email is already exists.");
                        }
                    }

                    //Admin email id 2
                    if (organizations.Customer_AdminEmailId2 != string.Empty)
                    {
                        User lmemail2 = new User();
                        lmemail2.Email = organizations.Customer_AdminEmailId2;
                        lmemail2.PasswordHash = auth.HashPassword("Testup@123");
                        lmemail2.Roleid = _context.Roles.Where(x => x.RoleName == "Admin" || x.RoleName == "admin").FirstOrDefaultAsync().Result.RoleId; ; ;
                        lmemail2.Organizationid = organizations.OrganizationId;
                        lmemail2.Level = level1id;
                        lmemail2.CreatedDate = DateTime.Now;
                        lmemail2.CreatedBy = _username;
                        var email4resgister = auth.RegisterNewUser(lmemail2);
                        if (email4resgister == 1)
                        {
                            ModelState.AddModelError("Customer_AdminEmailId4", "The email is already exists.");
                        }
                    }

                    //Admin email id 3
                    if (organizations.Customer_AdminEmailId3 != string.Empty)
                    {
                        User lmemail3 = new User();
                        lmemail3.Email = organizations.Customer_AdminEmailId3;
                        lmemail3.PasswordHash = auth.HashPassword("Testup@123");
                        lmemail3.Roleid = _context.Roles.Where(x => x.RoleName == "Admin" || x.RoleName == "admin").FirstOrDefaultAsync().Result.RoleId; ; ;
                        lmemail3.Organizationid = organizations.OrganizationId;
                        lmemail3.Level = level1id;
                        lmemail3.CreatedDate = DateTime.Now;
                        lmemail3.CreatedBy = _username;
                        var email5resgister = auth.RegisterNewUser(lmemail3);
                        if (email5resgister == 1)
                        {
                            ModelState.AddModelError("Customer_AdminEmailId5", "The email is already exists.");
                        }
                    }

                    #endregion
                    //   await SendWelcomeEmailAsync(organizations.Customer_AdminEmailId4, organizations.Customer_AdminEmailId4.Split('@')[0], "Testup@123");

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Exception_Msg", "Duplicate orgaization detail. Please check and do correct.");
            }
            return View(organizations);
        }

        private async Task SendWelcomeEmailAsync(string recipientEmail, string userName, string defaultpassword)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = true;
                string bodymsg = $"<h1>Welcome, {userName}!</h1><p>Thank you for registering with us. We're glad to have you on board!</p><p> Login detail: Email: {recipientEmail} </br> Password:{defaultpassword}</P";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = "Welcome to Our Service",
                    Body = bodymsg,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipientEmail);

                try
                {
                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine("Welcome email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send welcome email: {ex.Message}");
                }
            }
        }
        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizations = await _context.Organizations.FindAsync(id);
            if (organizations == null)
            {
                return NotFound();
            }

            return View(organizations);
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Organizations organizations)
        {
            if (id != organizations.OrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //emails created in organization table are level1
                    var level1id = _context.Levels.Where(x => x.LevelName == "Level 1" || x.LevelName == "Level1").FirstOrDefaultAsync().Result.LevelId;

                    #region logindetail
                    //Admin email id of carrergraph
                    if (organizations.CG_SuperAdminEmailId != string.Empty)
                    {
                        User lmemail1 = new User();
                        lmemail1.Email = organizations.CG_SuperAdminEmailId;
                        lmemail1.PasswordHash = auth.HashPassword("Testup@123");
                        lmemail1.Roleid = _context.Roles.Where(x => x.RoleName == "CG Superadmin" || x.RoleName == "CGSuperadmin").FirstOrDefaultAsync().Result.RoleId;
                        lmemail1.Organizationid = organizations.OrganizationId;
                        lmemail1.Level = level1id;
                        lmemail1.CreatedDate = DateTime.Now;
                        lmemail1.CreatedBy = _username;
                        var email1resgister = auth.RegisterNewUser(lmemail1);
                        if (email1resgister == 1)
                        {
                            return HandleModelError("Customer_AdminEmailId1", "The email already exists.", organizations);
                        }
                    }

                    //Admin email id for customer
                    if (organizations.Customer_SuperAdminEmailId != string.Empty)
                    {
                        User lmemail2 = new User();
                        lmemail2.Email = organizations.Customer_SuperAdminEmailId;
                        lmemail2.PasswordHash = auth.HashPassword("Testup@123");
                        lmemail2.Roleid = _context.Roles.Where(x => x.RoleName == "SuperAdmin" || x.RoleName == "Superadmin").FirstOrDefaultAsync().Result.RoleId;
                        lmemail2.Organizationid = organizations.OrganizationId;
                        lmemail2.Level = level1id;
                        lmemail2.CreatedDate = DateTime.Now;
                        lmemail2.CreatedBy = _username;
                        var email2resgister = auth.RegisterNewUser(lmemail2);
                        if (email2resgister == 1)
                        {
                            return HandleModelError("Customer_AdminEmailId2", "The email already exists.", organizations);
                        }
                    }

                    //Admin email id 1
                    if (organizations.Customer_AdminEmailId1 != string.Empty)
                    {
                        User lmemail3 = new User();
                        lmemail3.Email = organizations.Customer_AdminEmailId1;
                        lmemail3.PasswordHash = auth.HashPassword("Testup@123");
                        lmemail3.Roleid = _context.Roles.Where(x => x.RoleName == "Admin" || x.RoleName == "admin").FirstOrDefaultAsync().Result.RoleId; ;
                        lmemail3.Organizationid = organizations.OrganizationId;
                        lmemail3.Level = level1id;
                        lmemail3.CreatedDate = DateTime.Now;
                        lmemail3.CreatedBy = _username;
                        var email3resgister = auth.RegisterNewUser(lmemail3);
                        if (email3resgister == 1)
                        {
                            return HandleModelError("Customer_AdminEmailId3", "The email already exists.", organizations);
                        }
                    }

                    //Admin email id 2
                    if (organizations.Customer_AdminEmailId2 != string.Empty)
                    {
                        User lmemail4 = new User();
                        lmemail4.Email = organizations.Customer_AdminEmailId2;
                        lmemail4.PasswordHash = auth.HashPassword("Testup@123");
                        lmemail4.Roleid = _context.Roles.Where(x => x.RoleName == "Admin" || x.RoleName == "admin").FirstOrDefaultAsync().Result.RoleId; ; ;
                        lmemail4.Organizationid = organizations.OrganizationId;
                        lmemail4.Level = level1id;
                        lmemail4.CreatedDate = DateTime.Now;
                        lmemail4.CreatedBy = _username;
                        var email4resgister = auth.RegisterNewUser(lmemail4);
                        if (email4resgister == 1)
                        {
                            return HandleModelError("Customer_AdminEmailId4", "The email already exists.", organizations);
                        }
                    }

                    //Admin email id 3
                    if (organizations.Customer_AdminEmailId3 != string.Empty)
                    {
                        User lmemail5 = new User();
                        lmemail5.Email = organizations.Customer_AdminEmailId3;
                        lmemail5.PasswordHash = auth.HashPassword("Testup@123");
                        lmemail5.Roleid = _context.Roles.Where(x => x.RoleName == "Admin" || x.RoleName == "admin").FirstOrDefaultAsync().Result.RoleId; ; ;
                        lmemail5.Organizationid = organizations.OrganizationId;
                        lmemail5.Level = level1id;
                        lmemail5.CreatedDate = DateTime.Now;
                        lmemail5.CreatedBy = _username;
                        var email5resgister = auth.RegisterNewUser(lmemail5);
                        if (email5resgister == 1)
                        {
                            return HandleModelError("Customer_AdminEmailId5", "The email already exists.", organizations);
                        }
                    }

                    #endregion
                    _context.Update(organizations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationsExists(organizations.OrganizationId))
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
            return View(organizations);
        }


        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizations = await _context.Organizations
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organizations == null)
            {
                return NotFound();
            }

            return View(organizations);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organizations = await _context.Organizations.FindAsync(id);
            if (organizations != null)
            {
                _context.Organizations.Remove(organizations);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationsExists(int id)
        {
            return _context.Organizations.Any(e => e.OrganizationId == id);
        }

        private IActionResult HandleModelError(string propertyName, string errorMessage, Organizations organization)
        {
            if (propertyName != string.Empty)
            {
                ModelState.AddModelError(propertyName, errorMessage);
            }
            return View(organization);
        }
    }
}
