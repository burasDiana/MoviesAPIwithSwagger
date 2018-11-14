using DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace TestWebAPI.Security
{
    /// <summary>
    /// This class is used for operations that revolve around token generation, storing and accessing
    /// </summary>
    public class TokenHandler
    {
        public static string Value{ get; set; }
        static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static MoviesEntities db;

        static TokenHandler()
        {
           db = new MoviesEntities();
        }

        public static string GenerateRandomNumber(int length)
        {
            int nrOfChars = chars.Length;
            var stringChars = new char[length];
            Random rnd = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[rnd.Next(0, chars[nrOfChars - 1])];
            }
            return new string(stringChars);
        }

        public static void StoreToken(int userId, string Token)
        {
            db.Tokens.Add(new Token()
            {
                UserId = userId,
                TokenValue = Token
            });
            db.SaveChanges();
        }

        public static bool TokenExists(string token)
        {
            if (db.Tokens.Any(u => u.TokenValue.Equals(token)))
            {
                return true;
            }
            return false;
        }

        public static int GetUserID(string token)
        {
            return db.Tokens.FirstOrDefault(x => x.TokenValue == token).UserId;
        }
    }
}
