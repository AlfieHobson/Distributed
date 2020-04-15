using System;
using System.Text;
using CoreExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace DistSysACW.Controllers
{
    [Route("api/Protected")]
    public class ProtectedController : BaseController
    {
        public ProtectedController(Models.UserContext context) : base(context) { }

        // Takes key from header and replies Hello <username> taken from DB
        [HttpGet("Hello")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GETHello([FromHeader] string ApiKey)
        {
            // If key exists in database
            if (Models.UserDatabaseAccess.checkKey(_context, ApiKey))
            {
                Models.User user = Models.UserDatabaseAccess.getUser(_context, ApiKey);
                return Ok("Hello " + user.UserName);
            }
            return StatusCode(400, "Bad Request");
        }

        [HttpGet("sha1")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GETSha1([FromQuery] string message)
        {
            if (message != null)
            {
                using (SHA1 hashAlgo = SHA1.Create())
                {
                    return Ok(hashMessage(hashAlgo, message));
                }
            }
            return StatusCode(400, "Bad Request");
        }

        [HttpGet("sha256")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GETSha256([FromQuery] string message)
        {
            if (message != null)
            {
                using (SHA256 hashAlgo = SHA256.Create())
                {
                    return Ok(hashMessage(hashAlgo, message));
                }
            }
            return StatusCode(400, "Bad Request");
        }
        
        [HttpGet("getpublickey")]
        [Authorize(Roles ="Admin,User")]
        public IActionResult GETPublicKey()
        {
            string pubKey = RSA.getPublicKey();
            return Ok(pubKey);
        }

        [HttpGet("sign")]
        [Authorize(Roles ="Admin,User")]
        public IActionResult GETSign ([FromQuery] string message)
        {
            if (message == null) return StatusCode(400, "Bad Request");
            using (SHA1 hashAlgo = SHA1.Create())
            {
                // Hash Message
                string hash = hashMessage(hashAlgo, message);
                // Byte Message
                byte[] asciiByteMessage = Encoding.ASCII.GetBytes(hash);
                // Sign Message
                byte[] encryptedMessage = RSA.RSASign(asciiByteMessage);
                // Change back to string
                string encryptedString = ByteArrayToHexString(encryptedMessage);
                return Ok(encryptedString);
            }
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
                foreach (byte b in byteArray)  hexString += b.ToString("x2"); 
            } 
            return hexString; 
        }
    }

}