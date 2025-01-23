using Auxx.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Auxx.Controllers
{
    public abstract class CommonController : Controller
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IConfiguration _configuration;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected string? _level;
        protected string? _orgName;
        protected string? _roleName;
        protected string? _username;

        public CommonController(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            // Fetch session values and set ViewBag
            if (_httpContextAccessor != null)
            {
                // Fetch session values
                _level = _httpContextAccessor.HttpContext.Session.GetString("Level");
                _orgName = _httpContextAccessor.HttpContext.Session.GetString("OrganizationName");
                _roleName = _httpContextAccessor.HttpContext.Session.GetString("RoleName");
                //if (User != null)
                //{
                _username = User?.Identity?.Name ?? _httpContextAccessor.HttpContext.Session.GetString("Username");
                //}
            }
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            // Assign to ViewBag or ViewData
            ViewBag.Username = _username;
            ViewBag.loggedUserOrgName = _orgName;
            ViewBag.rolename = _roleName;
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

        public List<SelectListItem> GetOrganizationReportslist()
        {
            var reportlists = (from orgReport in _context.OrganizationReports
                               join report in _context.Reportdetails
                               on orgReport.ReportId equals report.ReportId
                               select new SelectListItem
                               {
                                   Value = report.ReportId.ToString(),
                                   Text = report.Name
                               }).ToList();

            return reportlists;
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

        public List<SelectListItem> GetLevellist()
        {
            //List<SelectListItem> levelList = new List<SelectListItem>();
            //levelList.Add(new SelectListItem() { Text = "Level 1", Value = "Level 1" });
            //levelList.Add(new SelectListItem() { Text = "Level 2", Value = "Level 2" });
            //levelList.Add(new SelectListItem() { Text = "Level 3", Value = "Level 3" });
            //levelList.Add(new SelectListItem() { Text = "All", Value = "All" });

            var levellist = _context.Levels.Select(r => new SelectListItem
            {
                Value = r.LevelId.ToString(),
                Text = r.LevelName
            }).ToList();

            return levellist;
        }

        public List<string> GetUserRoles(int userId)
        {
            // Replace with your actual database structure
            return (from ur in _context.User // UserRoles table linking UserId and RoleId
                    join r in _context.Roles on ur.Roleid equals r.RoleId
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
