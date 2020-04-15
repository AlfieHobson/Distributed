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
    [Route("api/talkback")]
    public class TalkBackController : BaseController
    {
        //private readonly ISomeService someService;

        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public TalkBackController(Models.UserContext context) : base(context) {
        }


        [HttpGet("Hello")]
        public string Get()
        {
            #region TASK1
            // TODO: add api/talkback/hello response
            #endregion
            return "Hello World";
        }

        [HttpGet("Sort")]
        public IActionResult Get([FromQuery]string[] integers)
        {
            #region TASK1
            // TODO: 
            /*try
            {
                
            }
            catch
            {
                return BadRequest();
            }
            return Ok(integers);*/
            // sort the integers into ascending order
            // send the integers back as the api/talkback/sort response
            #endregion
            foreach (string input in integers)
            {
                // Try to convert each input to integer. If fail, return bad request.
                try { int num = int.Parse(input); }
                catch (FormatException e) { return StatusCode(400,"Bad Request"); }
                catch (Exception e) { return StatusCode(400, "Bad Request"); }
            }
            Array.Sort(integers);
            return Ok(integers);
        }
    }
}
