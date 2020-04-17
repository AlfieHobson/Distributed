using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DistSysACWClient
{
    #region Task 10 and beyond
    public class Client
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello. What would you like to do?");

            string _username = "";
            string _key = "";
            string _publicKey = "";

            while (true)
            {
                // Read user input
                string[] input = Console.ReadLine().ToLower().Split(' ');

                string route;
                try
                {   
                    // Exit condition
                    if (input[0] == "exit") break;

                    // The first two words of the input = command.
                    string command = input[0] + " " + input[1];
                    Console.WriteLine("...please wait...");

                    // URL Parameters of request
                    List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

                    // Header information
                    List<KeyValuePair<string, string>> header = new List<KeyValuePair<string, string>>();

                    // Body information
                    Models.Body body = new Models.Body("");

                    // HTTP Response
                    Response response;

                    switch (command)
                    {
                        // talkback/hello
                        case ("talkback hello"):
                            route = "talkback/hello";
                            response = HTTPRequests.GetRequest(route,null).Result;
                            Console.WriteLine(response.Data);
                            break;


                        // talkback/sort
                        case ("talkback sort"):
                            route = "talkback/sort";

                            // Clean input
                            input[2] = input[2].Replace("[", "");
                            input[2] = input[2].Replace("]", "");
                            string[] numberArray = input[2].Split(',');

                            // Gather parameters
                            foreach (string number in numberArray)
                                parameters.Add(new KeyValuePair<string, string>("integers", number));
                            route = addParams(route, parameters);

                            response = HTTPRequests.GetRequest(route,null).Result;
                            Console.WriteLine(response.Data);
                            break;


                        // user/get
                        case ("user get"):
                            route = "user/new";
                            string username = input[2];
                            parameters.Add(new KeyValuePair<string, string>("username", username));
                            route = addParams(route, parameters);

                            response = HTTPRequests.GetRequest(route,null).Result;
                            Console.WriteLine(response.Data);
                            break;


                        // user/post
                        case ("user post"):
                            route = "user/new";
                            response = HTTPRequests.PostRequest(route, null, input[2]).Result;

                            if (response.StatusCode == "Forbidden")
                                Console.WriteLine(response.Data);
                            else if (response.StatusCode == "BadRequest")
                                Console.WriteLine(response.Data);
                            else if (response.StatusCode == "OK") {
                                Console.WriteLine("Got API Key");
                                _username = input[2];
                                _key = response.Data;
                                // Replace quotations that come back with it.
                                _key = _key.Replace("\"", "");
                            }
                            break;


                        // user/set
                        case ("user set"):
                            _username = input[2];
                            _key = input[3];
                            Console.WriteLine("Stored");
                            break;


                        case ("user delete"):
                            route = "user/removeuser";
                            // If no username set.
                            if (_username == "") {
                                Console.WriteLine("You need to do a User Post or User Set first");
                                break;
                            }

                            // Add username to URL
                            parameters.Add(new KeyValuePair<string, string>("username", _username));
                            route = addParams(route, parameters);

                            // Add key to header
                            header.Add(new KeyValuePair<string, string>("ApiKey", _key));

                            response = HTTPRequests.DeleteRequest(route, header).Result;
                            Console.WriteLine(response.Data);
                            break;


                        case ("user role"):
                            route = "user/changerole";
                            // If no key set.
                            if (_key == "") {
                                Console.WriteLine("You need to do a User Post or User Set first");
                                break;
                            }

                            // Add key to header
                            header.Add(new KeyValuePair<string, string>("key", _key));

                            // Add username and role to the body.
                            body.username = input[2];
                            // Capitalize first letter of role
                            body.role = char.ToUpper(input[3][0]) + input[3].Substring(1);
                            response = HTTPRequests.PostRequest(route, header, body).Result;
                            Console.WriteLine(response.Data);
                            break;


                        case ("protected hello"):
                            route = "protected/hello";
                            if (_key == "") {
                                Console.WriteLine("You need to do a User Post or User Set first");
                                break;
                            }
                            // Add key to header
                            header.Add(new KeyValuePair<string, string>("key", _key));

                            response = HTTPRequests.GetRequest(route,header).Result;
                            Console.WriteLine(response.Data);
                            break;


                        case ("protected sha1"):
                            route = "protected/sha1";
                            parameters.Add(new KeyValuePair<string, string>("message", input[2]));
                            if (_key == "") {
                                Console.WriteLine("You need to do a User Post or User Set first");
                                break;
                            }
                            // Add key to header
                            header.Add(new KeyValuePair<string, string>("key", _key));
                            // Add parameter to uri
                            route = addParams(route, parameters);

                            response = HTTPRequests.GetRequest(route, header).Result;
                            Console.WriteLine(response.Data);
                            break;


                        case ("protected sha256"):
                            route = "protected/sha256";
                            parameters.Add(new KeyValuePair<string, string>("message", input[2]));
                            if (_key == "") {
                                Console.WriteLine("You need to do a User Post or User Set first");
                                break;
                            }
                            // Add key to header
                            header.Add(new KeyValuePair<string, string>("key", _key));
                            // Add parameter to uri
                            route = addParams(route, parameters);

                            response = HTTPRequests.GetRequest(route, header).Result;
                            Console.WriteLine(response.Data);
                            break;

                        case ("protected get"):
                            route = "protected/getpublickey";
                            if (input[2] != "publickey") throw new IndexOutOfRangeException();
                            if (_key == "") {
                                Console.WriteLine("You need to do a User Post or User Set first");
                                break;
                            }
                            // Add key to header
                            header.Add(new KeyValuePair<string, string>("key", _key));

                            response = HTTPRequests.GetRequest(route, header).Result;
                            string responseKey = response.Data;// extractKey(response.Data);
                            if (responseKey == null) Console.WriteLine("Couldn’t Get the Public Key");
                            else
                            {
                                Console.WriteLine("Got Public Key");
                                _publicKey = responseKey;
                            }
                            break;

                        case ("protected sign"):
                            // CHECK FOR PUBLIC KEY

                            route = "protected/sign";
                            if (_key == "") {
                                Console.WriteLine("Client doesn’t yet have the public key");
                                break;
                            }
                            // Add key to header
                            header.Add(new KeyValuePair<string, string>("key", _key));
                            // Add parameter to uri
                            parameters.Add(new KeyValuePair<string, string>("message", input[2])); 
                            route = addParams(route, parameters);

                            // Get Signed message from server
                            response = HTTPRequests.GetRequest(route, header).Result;

                            // Convert message to bytes
                            byte[] message = Encoding.ASCII.GetBytes(input[2]);
                            // Server Response
                            string cleanedReponse = removeDelimeters(response.Data);
                            byte[] serverResponse = StringToByteArray(cleanedReponse);

                            // Verify Signature
                            bool verified = RSA.RSAVerify(message, serverResponse, _publicKey);

                            if (verified) Console.WriteLine("Message was successfully signed");
                            else Console.WriteLine("Message was not successfully signed");
                            break;
                    }

                    Console.WriteLine("\r\nWhat would you like to do next?");
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("ERROR: Missing Argument");
                }
            }
        }


        // Appends parameters to Url.
        private static string addParams(string route, List<KeyValuePair<string, string>> parameters)
        {
            if (parameters != null)
            {
                route += '?';
                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    route += parameter.Key + '=' + parameter.Value + '&';
                }
                return route;
            }
            return route;
        }

        // Given an algorithm, will return the hash of a message.
        private static string hashMessage(HashAlgorithm hashAlgo, string message)
        {
            byte[] sourceBytes = Encoding.UTF8.GetBytes(message);
            byte[] hashBytes = hashAlgo.ComputeHash(sourceBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            return hash;
        }

        private static string ByteArrayToHexString(byte[] byteArray)
        {
            string hexString = "";
            if (null != byteArray)
            {
                foreach (byte b in byteArray) hexString += b.ToString("x2");
            }
            return hexString;
        }
        public static byte[] StringToByteArray(String hex) 
        { 
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes; 
        }

        public static string removeDelimeters (string data)
        {
            return data.Replace("-", "");
        }
    }
    #endregion
}
