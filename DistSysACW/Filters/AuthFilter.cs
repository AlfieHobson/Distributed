using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACW.Filters
{
    [Authorize(Roles = "Admin")]
    public class AuthFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string roleNeeded = "";
            try
            {
                AuthorizeAttribute authAttribute = (AuthorizeAttribute)context.ActionDescriptor.EndpointMetadata.Where(e => e.GetType() == typeof(AuthorizeAttribute)).FirstOrDefault();

                if (authAttribute != null)
                {
                    string[] roles = authAttribute.Roles.Split(',');
                    foreach (string role in roles)
                    {
                        roleNeeded = role;
                        if (context.HttpContext.User.IsInRole(role))
                        {
                            return;
                        }
                    }          
                    throw new UnauthorizedAccessException();
                }
            }
            catch
            {
                // Unauthorised status 401.
                context.HttpContext.Response.StatusCode = 401;
                // Check which role they failed to meet.
                switch (roleNeeded) {
                    case ("Admin"):
                        context.Result = new JsonResult("Unauthorized. Admin access only.");
                        break;
                    case ("User"):
                        context.Result = new JsonResult("Unauthorized. User access required.");
                        break;
                    default:
                        context.Result = new JsonResult("Unauthorized. Check ApiKey in Header is correct.");
                        break;
                }
            }
        }
    }
}
