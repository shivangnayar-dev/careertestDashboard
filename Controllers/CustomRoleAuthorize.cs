using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

//public class CustomRoleAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
//{
//    public string AllowedRole { get; set; }

//    public override void OnAuthorization(AuthorizationFilterContext context)
//    {
//        var user = context.HttpContext.User;

//        if (!user.Identity.IsAuthenticated || !user.IsInRole(AllowedRole))
//        {
//            context.Result = new UnauthorizedResult();
//        }
//    }
//}