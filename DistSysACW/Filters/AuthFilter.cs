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
    public class AuthFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                AuthorizeAttribute authAttribute = (AuthorizeAttribute)context.ActionDescriptor.EndpointMetadata.Where(e => e.GetType() == typeof(AuthorizeAttribute)).FirstOrDefault();

                if (authAttribute != null)
                {
                    string[] roles = authAttribute.Roles.Split(',');
                    foreach (string role in roles)
                    {
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
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult("Unauthorized. Check ApiKey in Header is correct.");
            }
        }
    }
}
