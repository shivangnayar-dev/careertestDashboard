using Auxx.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Configuration;
using System.Security.Claims;
//using System.Web.Mvc;

namespace Auxx.Controllers
{
    //[Route("Dashboard")]
    [Authorize]
    // [CustomRoleAuthorize(Roles = "Admin, Manager")]

    public class DashboardController : CommonController
    {
        private readonly DatabaseRepositorycContext _repository;
        
        private readonly string _connectionString;

        public DashboardController(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor) // Call the CommonController constructor
        {
            //TODO: currently rolename value is not passed in paramter just passing empty to get only 2 clients data for CG Superadmin role
            _repository = new DatabaseRepositorycContext(configuration, _orgName, _roleName);
            //_repository = new DatabaseRepositorycContext(configuration);
        }

        // GET: Dashboard
        [Authorize]
        public IActionResult Index()
        {
            //if (HttpContext.Session["Username"] == null)
            //{
            //  return RedirectToAction("LoginBasic", "Auth"); // Redirect to login if session is not set
            //}
            //ViewBag.Username = Session["Username"];
           // _repository = new DatabaseRepositorycContext(configuration, _orgName, _roleName);
            return View();
        }
        public IActionResult Analytics()
        {
            return View();
        }

        //[HttpGet("GetClientSatisfactionData")]
        public JsonResult GetTestStageData()
        {
            var listdata = _repository.GetTestStageDBData();
            var data = new List<object>();
            if (listdata.Count > 0)
            {
                data = new List<object>
                {
                    new { label = "Register", series = listdata[0] },
                    new { label = "Test", series = listdata[1] },
                    new { label = "Completed", series = listdata[2] }
                };
            }
            // return data as json result
            return Json(data);
        }


        public JsonResult GetTestAttemptsData()
        {
            var listdata = _repository.GetTestAttemptsDBData();
            var data = new List<object>();
            if (listdata.Count > 0)
            {
                data = new List<object>
                {
                    new { label = "1", series = listdata[0] },
                    new { label = "2+", series = listdata[1] == null ? 0 : listdata[1] },
                };
            }
            // return data as json result
            return Json(data);
        }

        public JsonResult GetTestSummarytData()
        {
            var listdata = _repository.GetTestSummaryDBData();

            // return data as json result
            //return Json(listdata);
            return Json(new { success = true, data = listdata });
        }

        public JsonResult GetTestSummarytDetailData(string testcode)
        {
            var listdata = _repository.GetTestSummaryDetailsDBData(testcode);


            // return data as json result
            //   return Json(listdata);
            return Json(new { success = true, details = listdata });
        }

        public JsonResult GetTestPerdayMonthwiseData()
        {
            var listdata = _repository.GetTestPerdayMonthwiseDBData();


            // return data as json result
            return Json(listdata);
        }

        public JsonResult GetTestCountBlockData()
        {
            var listdata = _repository.GetTestUserCounts();

            // return data as json result
            return Json(listdata);
        }

        public JsonResult GetCompletedCountBlockData()
        {
            var listdata = _repository.GetCompletedUserCounts();

            // return data as json result
            return Json(listdata);
        }

        [HttpGet]
        public JsonResult GetNewUsersCountBlockData()
        {
            var listdata = _repository.GetNewUserCounts();

            // return data as json result
            return Json(listdata);
        }

        public JsonResult GetRepeatLoginCountBlockData()
        {
            var listdata = _repository.GetRepeatLoginCounts();

            // return data as json result
            return Json(listdata);
        }

        public FileResult DownloadReport(string reportPath)
        {
            if (string.IsNullOrEmpty(reportPath))
            {
                throw new FileNotFoundException("Report not found");
            }

            string fullPath = reportPath;//Microsoft.AspNetCore.Hosting.Server.MapPath(reportPath); // Replace this with your actual file storage location
            string fileName = Path.GetFileName(fullPath);

            if (!System.IO.File.Exists(fullPath))
            {
                //return new HttpStatusCodeResult(HttpStatusCode.NotFound,"File not found");
            }

            return File(fullPath, "application/octet-stream", fileName);
        }
    }
}