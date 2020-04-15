using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DistSysACW.Models
{
    public class User
    {
        #region Task2
        // TODO: Create a User Class for use with Entity Framework
        // Note that you can use the [key] attribute to set your ApiKey Guid as the primary key 
        #endregion
        [Key]
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public User (){}
    }

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion

    public static class UserDatabaseAccess
    {
        #region Task3 
        /// <summary>
        /// Creates a user given a name and role.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="name"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static string createUser(UserContext dbContext, string name)
        {
            string role;
            //If first user, set role to "Admin", else "User" role.
            var studentList = dbContext.Users.ToList();
            if (studentList.Count == 0)
                role = "Admin";
            else
                role = "User";

            // Create User object
            User newUser = new User()
            {
                UserName = name,
                Role = role
            };
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();

            return newUser.ApiKey;
        }

        /// <summary>
        /// Collects user based of apiKey
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static User getUser (UserContext dbContext, string key)
        {
            // Query database for user.
            User user = dbContext.Users.FirstOrDefault(u => u.ApiKey == key);

            // If query fails, return false.
            if (user == null)
                return null;
            else
                return user;
        }

        /// <summary>
        /// Deletes user based of key.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool deleteUser(UserContext dbContext, string key)
        {
            // Get user
            User user = dbContext.Users.FirstOrDefault(u => u.ApiKey == key);
            if (user != null)
            {
                // Remove user
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool changeRole(UserContext dbContext, string name, string role)
        {
            try
            {
                // Get User
                User user = dbContext.Users.FirstOrDefault(u => u.UserName == name);
                user.Role = role;
                dbContext.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Uses checkKey & checkName
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool checkKeyandName(UserContext dbContext, string key, string name)
        {
            // Query database for key
            var query = dbContext.Users.Where(s => s.ApiKey == key)
                           .FirstOrDefault<User>();
            // If name in db matches name given, return True
            if (query.UserName == name)
                return true;
            return false;
        }

        /// <summary>
        /// Check if a db entry exists based on API KEY
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static bool checkKey(UserContext dbContext, string key)
        {
            User user = dbContext.Users.FirstOrDefault(u => u.ApiKey == key);
            if (user == null)
                return false;
            return true;
        }

        /// <summary>
        ///  Check if a db entry exists based on a username
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool checkName(UserContext dbContext, string name)
        {
            var query = dbContext.Users.Where(s => s.UserName == name)
                           .FirstOrDefault<User>();

            // Query returns null if no entrys match the search.
            if (query != null)
                return true;
            else
                return false;
        }
        #endregion

    }


}