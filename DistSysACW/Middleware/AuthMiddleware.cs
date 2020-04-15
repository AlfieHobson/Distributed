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
            // Get Key
            var headerInfo = context.Request.Headers["ApiKey"].ToString();
            // Get user based on Key
            Models.User currentUser = Models.UserDatabaseAccess.getUser(dbContext, headerInfo);

            // If a user was found, add claims
            if (currentUser != null)
            {
                var claim = new Claim(ClaimTypes.Role, currentUser.Role);
                var name = new Claim(ClaimTypes.Name, currentUser.UserName);
                var keyClaim = new Claim(ClaimTypes.NameIdentifier, headerInfo);
                context.User.AddIdentity(new ClaimsIdentity(new[] { claim, name, keyClaim }));
            }

            #endregion

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

    }
}
