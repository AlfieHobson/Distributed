using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DistSysACW.Controllers
{
    [Route("api/User")]
    public class UserController : BaseController
    {
        public UserController(Models.UserContext context) : base(context){}

        [HttpGet("New")]
        // Trying to create a user through a GET request. Did they mean Post?
        public IActionResult Get([FromQuery]string username)
        {
            // If name exists.
            if (username != null)
            {
                // If user exists
                if (Models.UserDatabaseAccess.checkName(_context, username))
                    return Ok("True - User Does Exist! Did you mean to do a POST to create a new user?");
                else
                    return
                        Ok("False - User Does Not Exist! Did you mean to do a POST to create a new user?");
            }
            else
                return Ok("False - User Does Not Exist! Did you mean to do a POST to create a new user?");
        }

        [HttpPost("New")]
        public IActionResult Post([FromBody]string username)
        {

            // If no string is submitted in the body, return fail.
            if (username == null || username == "")
                return StatusCode(400, "Oops. Make sure your body contains a string with your username and your Content-Type is Content-Type:application/json");
            // Check if the username is taken
            if (Models.UserDatabaseAccess.checkName(_context, username))
                return StatusCode(403,"Oops.This username is already in use.Please try again with a new username.");
            else
            {
                // Create entry
                string key = Models.UserDatabaseAccess.createUser(_context, username);
                return Ok(key);
            }
        }


        [HttpDelete("RemoveUser")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Delete([FromQuery]string username, [FromHeader]string key)
        {
            // Check if key exists.
            if(Models.UserDatabaseAccess.checkKey(_context,key))
            {
                // Check if key and username Match
                if(Models.UserDatabaseAccess.checkKeyandName(_context,key,username))
                {
                    // Delete
                    if (Models.UserDatabaseAccess.deleteUser(_context, key))
                        return Ok(true);
                }
            }
            return Ok(false);
        }

        [HttpPost("ChangeRole")]
        [Authorize(Roles ="Admin")]
        public IActionResult PostRole([FromBody]Dictionary<string,string> body)
        {
            string username = body["username"];
            string role = body["role"];
            // Validate username
            if (body == null)
                return StatusCode(400, "NOT DONE: Username does not exist");

            // Validate role
            else if (role != "user" && role != "admin")
                return StatusCode(400, "NOT DONE: Role does not exist");
            else
            {
                if(Models.UserDatabaseAccess.changeRole(_context,username,role))
                    return Ok("DONE");
            }
            // All other errors
            return StatusCode(400, "NOT DONE: An error occured");
        }
    }
}
