using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// Called from Configure in Startup.cs
// Whenever a HTTP request is made, this method is run first, checking the API key in the header of the message is correct.
namespace DistSysACW.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Models.UserContext dbContext)
        {
            #region Task5
            // TODO:  Find if a header ‘ApiKey’ exists, and if it does, check the database to determine if the given API Key is valid
            //        Then set the correct roles for the User, using claims
            #endregion

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

    }
}
