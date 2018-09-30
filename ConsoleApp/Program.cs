using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = GenerateRandomAlphaNumericKey(23);
            Console.WriteLine("Random key is:" + key);
            Console.WriteLine("To copy: " + EncodeHashToBase64("Alex" ,Generatehash("Alex" , "123" , key , "mdk"), key));
           
            Console.ReadLine();
        }
        private static string Generatehash(string username, string password, string key, string domain)
        {
            var hash = String.Format(
                    "{0}:{1}:{2}:{3}" ,
                    username ,
                    password ,
                    key,
                    domain).ToMd5Hash();
            return hash;
        }

        private static string EncodeHashToBase64(string username, string hash, string key)
        {
            string authstring = username + ":" +  key + ":" + hash;
            Console.WriteLine(authstring);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(authstring);
            var converted = System.Convert.ToBase64String(plainTextBytes);
            Console.WriteLine("Converted string"+ converted);
            return converted ;

        }

        private static string GenerateRandomAlphaNumericKey(int length)
        {
          Random random = new Random();
          const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
          return new string(Enumerable.Repeat(chars , length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static Credentials ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' },2);

            if (string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]) )
                return null;
            return new Credentials() { Username = credentials[0] , Password = credentials[1]  };
        }

        private static bool CheckUsername(string username)
        {
            if ( username.Contains("@") && username.Contains(".com") )
            {
                return true;
            }
            return false;
        }
    }
}
