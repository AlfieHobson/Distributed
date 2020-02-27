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
        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public TalkBackController(Models.UserContext context) : base(context) { }


        [ActionName("Hello")]
        public string Get()
        {
            return "Hello World";
            #region TASK1
            // TODO: add api/talkback/hello response
            #endregion
        }

        /*[ActionName("Sort")]
        public IActionResult Get([FromQuery]int[] integers)
        {

            #region TASK1
            // TODO: 
            // sort the integers into ascending order
            // send the integers back as the api/talkback/sort response
            #endregion
        }*/
    }
}
