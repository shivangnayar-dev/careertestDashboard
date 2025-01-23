using Auxx.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auxx.Controllers
{
    public class UsersController : CommonController
    {
        //AuthController auth = null;

        public UsersController(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor) // Call the CommonController constructor
        {
            //auth = new AuthController(context, configuration, httpContextAccessor);
        }

        // GET: Users
        public IActionResult Grid()
        {
            return View();
        }
        public async Task<IActionResult> List()
        {
            ViewData["RolesList"] = GetRoleslist();
            ViewData["OrganizationsList"] = GetOrganizationslist();
            ViewData["LevelList"] = GetLevellist();
            return View(await _context.User.ToListAsync());
            //return View();
        }

        [Authorize] // Ensure only authenticated users can access this endpoint
        [HttpPost]
        //[Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input data");
            }

            // Find the user from the database
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == model.Id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update the user details
            //user.Email = model.Email;
            user.Roleid = model.Roleid;
            user.Mobile = model.Mobile;
            user.Organizationid = model.Organizationid;
            user.Adhar = model.Adhar;
            //user.IsActive = model.IsActive;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("User details updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}