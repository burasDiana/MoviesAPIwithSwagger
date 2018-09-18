using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace TestWebAPI.Security
{
    public class Token
    {
        public static string Value{ get; set; }
        public static Dictionary<int, string> dict;
        static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        static Token()
        {
            dict = new Dictionary<int, string>();
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
            dict.Add(userId,Token);
        }
    }
}