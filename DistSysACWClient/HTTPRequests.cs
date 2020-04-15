using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DistSysACWClient
{
    public static class HTTPRequests
    {
        //private static string page = "http://distsysacw.azurewebsites.net/9448017/Api/";
        private static string page = "https://localhost:44307/api/";

        public static async Task<Response> GetRequest(string route, List<KeyValuePair<string, string>> header)
        {
            string domain = page + route;

            using (HttpClient client = new HttpClient())
            {
                if (header != null) client.DefaultRequestHeaders.Add("ApiKey", header[0].Value);
                using (HttpResponseMessage response = await client.GetAsync(domain))
                using (HttpContent content = response.Content)
                {
                    // Wait for response
                    string data = await content.ReadAsStringAsync();

                    var status = response.StatusCode;
                    Response GETResponse = new Response(data, response.StatusCode.ToString());
                    return GETResponse;
                }
            }
        }

        //Post request Overload 1. Takes an object to put in body.
        public static async Task<Response> PostRequest(string route, List<KeyValuePair<string, string>> header, Models.Body body)
        {
            string domain = page + route;

            // Convert our class into JSON
            var json = await Task.Run(() => JsonConvert.SerializeObject(body));

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                using (HttpClient client = new HttpClient())
                {
                    if (header != null) client.DefaultRequestHeaders.Add("ApiKey", header[0].Value);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (HttpResponseMessage response = await client.PostAsync(domain, httpContent))
                    using (HttpContent content = response.Content)
                    {
                        // Wait for response
                        string data = await content.ReadAsStringAsync();

                        var status = response.StatusCode;
                        Response POSTResponse = new Response(data, response.StatusCode.ToString());
                        return POSTResponse;
                    }
                }
            }
        }
        //Post request Overload 2. Takes a string to put in body.
        public static async Task<Response> PostRequest(string route, List<KeyValuePair<string, string>> header, string body)
        {
            string domain = page + route;

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            using (var stringContent = new StringContent($"\"{body}\"", Encoding.UTF8, "application/json")) 
            {
                using (HttpClient client = new HttpClient())
                {
                    if (header != null) client.DefaultRequestHeaders.Add("key", header[0].Value);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (HttpResponseMessage response = await client.PostAsync(domain, stringContent))
                    using (HttpContent content = response.Content)
                    {
                        // Wait for response
                        string data = await content.ReadAsStringAsync();

                        var status = response.StatusCode;
                        Response POSTResponse = new Response(data, response.StatusCode.ToString());
                        return POSTResponse;
                    }
                }
            }
        }

        public static async Task<Response> DeleteRequest(string route, List<KeyValuePair<string, string>> header)
        {
            string domain = page + route;         

            using (HttpClient client = new HttpClient())
            {
                if (header != null) client.DefaultRequestHeaders.Add("ApiKey", header[0].Value);

                using (HttpResponseMessage response = await client.DeleteAsync(domain))
                using (HttpContent content = response.Content)
                {
                    // Wait for response
                    string data = await content.ReadAsStringAsync();

                    var status = response.StatusCode;
                    Response DELETEResponse = new Response(data, response.StatusCode.ToString());
                    return DELETEResponse;
                }
            }
        }
    }
}
