using Auxx.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;


namespace Auxx.Controllers
{
    public class AuthController : CommonController
    {

        private readonly string _connectionString;
        // private readonly string _orgName;

        public AuthController(ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(context, configuration, httpContextAccessor) // Call the CommonController constructor
        {
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // GET: Auth
        public ActionResult CreatePasswordBasic()
        {
            return View();
        }
        public ActionResult CreatePasswordCover()
        {
            return View();
        }
        public ActionResult LoginBasic()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LogIn([FromBody] User model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.PasswordHash))
            {
                return Json(new { success = false, message = "Username and password are required." });
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var passwordHash = string.Empty;
                var query = @"SELECT PasswordHash, u.RoleId, u.organizationid, r.Rolename, o.organizationname,u.Id, l.levelname FROM User u 
                            INNER JOIN Roles r on u.RoleId = r.RoleId 
                            INNER JOIN Organizations o on u.OrganizationId = o.OrganizationId
                            INNER JOIN levels l on l.levelId = u.level
                            WHERE u.Email = @Username";
                var claims = new List<Claim>();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", model.Username);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            passwordHash = reader.GetString("PasswordHash"); // command.ExecuteScalar() as string;
                                                                             // Add claims for Role and Organization
                            var roles = GetUserRoles(reader.GetInt32("roleid")); // Fetch roles for the user
                            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                            claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, model.Username),
                                new Claim("RoleId", reader.GetInt32("RoleId").ToString()),
                                new Claim("OrganizationId", reader.GetInt32("organizationid").ToString()),
                                new Claim("RoleName", reader.GetString("Rolename")),
                                new Claim("OrganizationName", reader.GetString("organizationname")),
                                new Claim("Level", reader.GetString("levelname"))
                            };

                            HttpContext.Session.SetString("Username", model.Username);
                            HttpContext.Session.SetInt32("RoleId", reader.GetInt32("RoleId"));
                            HttpContext.Session.SetInt32("OrganizationId", reader.GetInt32("organizationid"));
                            HttpContext.Session.SetString("RoleName", reader.GetString("Rolename"));
                            HttpContext.Session.SetString("OrganizationName", reader.GetString("organizationname"));
                            HttpContext.Session.SetString("Level", reader.GetString("levelname"));
                        }
                    }
                }
                if (passwordHash != null && VerifyPasswordHash(model.PasswordHash, passwordHash))
                {
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return Json(new { success = true, message = "Signin successful." });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid username or password." });
                }
            }
        }

        // Helper function to verify a hashed password
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hash == storedHash.ToLower();
            }
        }

        public ActionResult LoginCover()
        {
            return View();
        }
        public ActionResult LogoutBasic()
        {
            return View();
        }
        public ActionResult LogoutCover()
        {
            // Clear session
            //Session.Clear();
            //Session.Abandon();

            //// Clear authentication cookie
            //Response.Cookies[".AspNet.ApplicationCookie"].Expires = DateTime.Now.AddDays(-1);

            // Redirect to login page
            return RedirectToAction("LoginBasic", "Auth");
            //return View();
        }
        public ActionResult RegisterBasic()
        {
            ViewData["RolesList"] = GetRoleslist();
            ViewData["OrganizationsList"] = GetOrganizationslist();
            ViewData["LevelList"] = GetLevellist();
            return View();
        }
        [HttpPost]
        public JsonResult Register([FromBody] User model)
        {
            var savesuccess = RegisterNewUser(model);
            if(savesuccess == 0) {
                return Json(new { success = false, message = "All fields are required." });
            }
            else if (savesuccess == 1){
                Json(new { success = false, message = "Email already exists." });
            }

            return Json(new { success = true, message = "Registration successful." });
        }

        public int RegisterNewUser(User model)
        {
            //if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Roleid.ToString()) || string.IsNullOrEmpty(model.Organizationid.ToString()))
            if (string.IsNullOrEmpty(model.PasswordHash) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Roleid.ToString()) || string.IsNullOrEmpty(model.Organizationid.ToString()))
            {
                return 0;// Json(new { success = false, message = "All fields are required." });
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Check if username or email already exists
                var query = @"SELECT COUNT(*) as cntUser, roleId, organizationid, Level FROM User 
                            WHERE  Email = @Email AND roleid = @RoleId AND organizationId = @OrganizationId AND level=@Level";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@RoleId", model.Roleid);
                    command.Parameters.AddWithValue("@OrganizationId", model.Organizationid);
                    command.Parameters.AddWithValue("@Level", model.Level);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int count = Convert.ToInt32(reader.GetInt32("cntUser"));
                            if (count > 0)
                            {
                                return 1;
                            }
                        }
                    }
                }

                // Insert the new user into the database
                string hashedPassword = HashPassword(model.PasswordHash);
                //  string insertQuery = "INSERT INTO Users (Username, PasswordHash, Email) VALUES (@Username, @PasswordHash, @Email)";
                string insertQuery = "INSERT INTO User (PasswordHash, Email, Mobile, Adhar, Roleid, OrganizationId,level) VALUES (@PasswordHash, @Email, @Mobile, @Adhar, @RoleId, @OrganizationId,@level)";
                using (var insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    // insertCommand.Parameters.AddWithValue("@Username", model.Username);
                    insertCommand.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    insertCommand.Parameters.AddWithValue("@Email", model.Email);
                    insertCommand.Parameters.AddWithValue("@Mobile", model.Mobile);
                    insertCommand.Parameters.AddWithValue("@Adhar", model.Adhar);
                    insertCommand.Parameters.AddWithValue("@RoleId", model.Roleid);
                    insertCommand.Parameters.AddWithValue("@OrganizationId", model.Organizationid);
                    insertCommand.Parameters.AddWithValue("@level", model.Level);
                    insertCommand.ExecuteNonQuery();
                }
                return 2;
            }
        }

        // Helper method for hashing passwords
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public ActionResult RegisterCover()
        {
            return View();
        }
        public ActionResult ResetPasswordBasic()
        {
            return View();
        }
        public ActionResult ResetPasswordCover()
        {
            return View();
        }
        public ActionResult TwoStepsBasic()
        {
            return View();
        }
        public ActionResult TwoStepsCover()
        {
            return View();
        }
        public ActionResult VerifyEmailBasic()
        {
            return View();
        }
        public ActionResult VerifyEmailCover()
        {
            return View();
        }
    }
}