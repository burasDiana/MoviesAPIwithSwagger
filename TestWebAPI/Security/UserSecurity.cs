using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;

namespace TestWebAPI.Security
{
    public class UserSecurity
    {
        public enum UserType
        {
            Undefined = -1,
            Admin = 0,
            Customer = 1
        }

        public static string GetPasswordForUser(string username)
        {
            using ( MoviesEntities db = new MoviesEntities() )
            {
                return db.Users.FirstOrDefault(u => u.Username.Equals(username , StringComparison.Ordinal)).Password;
            }
        }

        public static bool Login(string username, string password)
        {
        using(MoviesEntities entities = new MoviesEntities() )
            {
                return entities.Users.Any(u => u.Username.Equals(username , StringComparison.OrdinalIgnoreCase) && u.Password == password);
            }
        }

        public static int GetUserId(string username)
        {
            using (MoviesEntities entities = new MoviesEntities())
            {
                if(entities.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    return entities.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).Id;
                }
                return -1;
            }
        }

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