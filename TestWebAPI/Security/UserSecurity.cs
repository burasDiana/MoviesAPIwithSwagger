using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;

namespace TestWebAPI.Security
{
    /// <summary>
    /// This class is used for operations that revolve around users
    /// </summary>
    public class UserSecurity
    {
        /// <summary>
        /// enum defining the types of user that can access the API
        /// </summary>
        public enum UserType
        {
            Undefined = -1,
            Admin = 0,
            Customer = 1
        }

        /// <summary>
        /// Gets the password of a certain user
        /// </summary>
        public static string GetPasswordForUser(string username)
        {
            using (MoviesEntities db = new MoviesEntities())
            {
                return db.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.Ordinal)).Password;
            }
        }

        /// <summary>
        /// Verifies that a user with the matching username/password exists in the database
        /// </summary>
        public static bool Login(string username, string password)
        {
            using (MoviesEntities entities = new MoviesEntities())
            {
                return entities.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            }
        }

        /// <summary>
        /// Verifies that a user with the matching username exists in the database
        /// </summary>
        public static bool FindUser(string username)
        {
            using (MoviesEntities entities = new MoviesEntities())
            {
                return entities.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Gets the device token associated with a user
        /// </summary>
        public static string GetDeviceTokenByUserId(int userId,out bool success)
        {
            success = false;
            using (MoviesEntities entities = new MoviesEntities())
            {
                var deviceToken = entities.Users.FirstOrDefault(u => u.Id.Equals(userId)).Password;

                if (deviceToken != null)
                {
                    success = true;
                    return deviceToken;
                }
                return "";
            }
        }

        /// <summary>
        /// Verifies that a user is also an admin
        /// </summary>
        public static bool IsAdminUser(int userId)
        {
            using (MoviesEntities entities = new MoviesEntities())
            {

                var isAdmin = entities.Users.FirstOrDefault(u => u.Id.Equals(userId)).IsAdmin;

                if (isAdmin != null && isAdmin.HasValue)
                {
                    return isAdmin.Value;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the user id based on the user name
        /// </summary>
        public static int GetUserId(string username)
        {
            using (MoviesEntities entities = new MoviesEntities())
            {
                if (entities.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    return entities.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).Id;
                }
                return -1;
            }
        }

        /// <summary>
        /// Gets the username of a user based on the user id
        /// </summary>
        public static string GetUserName(int userId)
        {
            using (MoviesEntities entities = new MoviesEntities())
            {
                if (entities.Users.Any(u => u.Id.Equals(userId)))
                {
                    return entities.Users.FirstOrDefault(u => u.Id.Equals(userId)).Username;
                }
                return "not_Found";
            }
        }
    }
}