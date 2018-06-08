using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;

namespace TestWebAPI.Security
{
    public class UserSecurity
    {
        public static bool Login(string username, string password)
        {
        using(MoviesEntities entities = new MoviesEntities() )
            {
                return entities.Users.Any(u => u.Username.Equals(username , StringComparison.OrdinalIgnoreCase) && u.Password == password);
            }
        }
    }
}