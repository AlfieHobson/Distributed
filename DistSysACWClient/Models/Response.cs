using System;
using System.Collections.Generic;
using System.Text;

namespace DistSysACWClient
{
    public class Response
    {
        public string Data { get; set; }
        public string StatusCode { get; set; }

        public Response (string data, string statusCode)
        {
            Data = data;
            StatusCode = statusCode;
        }

    }
}
