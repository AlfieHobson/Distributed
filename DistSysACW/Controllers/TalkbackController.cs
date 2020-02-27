using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DistSysACW.Controllers
{
    [Route("api/talkback/")]
    public class TalkBackController : BaseController
    {
        private readonly ISomeService someService;

        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public TalkBackController(Models.UserContext context, ISomeService someService) : base(context) {
            this.someService = someService;
        }


        [HttpGet("Hello")]
        public string Get()
        {
            someService.Hello("Hello get method");
            return "Hello World1";
            #region TASK1
            // TODO: add api/talkback/hello response
            #endregion
        }

        [HttpGet("Sort")]
        public IActionResult Get([FromQuery]int[] integers)
        {
            Array.Sort(integers);
            return Ok(integers);

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
        }
    }
}
