using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Auxx.Controllers
{
    public abstract class CommonController : Controller
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IConfiguration _configuration;

        public CommonController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<SelectListItem> GetRoleslist()
        {
            var roles = _context.Roles.Select(r => new SelectListItem
                            {
                                Value = r.RoleId.ToString(),
                                Text = r.RoleName
                            }).ToList();

            return roles;
        }
        public List<SelectListItem> GetOrganizationslist()
        {
            var organizations = _context.Organizations.Select(r => new SelectListItem
            {
                Value = r.OrganizationId.ToString(),
                Text = r.OrganizationName
            }).ToList();

            return organizations;
        }

        public List<SelectListItem> GetReportslist()
        {
            var reportlist = _context.Reportdetails.Select(r => new SelectListItem
            {
                Value = r.ReportId.ToString(),
                Text = r.Name
            }).ToList();

            return reportlist;
        }

        public List<SelectListItem> GetAccesslist()
        {
            List<SelectListItem> levelList = new List<SelectListItem>();
            levelList.Add(new SelectListItem() { Text = "Level 1", Value = "Level 1" });
            levelList.Add(new SelectListItem() { Text = "Level 2", Value = "Level 2" });
            levelList.Add(new SelectListItem() { Text = "Level 3", Value = "Level 3" });
            //levelList.Add(new SelectListItem() { Text = "All", Value = "All" });


            return levelList;
        }

        public List<string> GetUserRoles(int userId)
        {
            // Replace with your actual database structure
            return (from ur in _context.User // UserRoles table linking UserId and RoleId
                    join r in _context.Roles on ur.RoleId equals r.RoleId
                    where ur.Id == userId
                    select r.RoleName).ToList();
        }

        //public string[] GetSessionList()
        //{
        //    var sessionlis = new string[5] {
        //        HttpContext.Session.GetString("Username"),
        //        HttpContext.Session.GetInt32("RoleId").ToString(),
        //        HttpContext.Session.GetInt32("OrganizationId").ToString(),
        //        HttpContext.Session.GetString("RoleName"),
        //        HttpContext.Session.GetString("OrganizationName")
        //    };

        //    return sessionlis;
        //}
    }
}
