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
            //Console.WriteLine("Insert username:");
            //var username = Console.ReadLine();
            //while (!CheckUsername(username) )
            //{
            //    Console.WriteLine("Username must be a valid email address:");
            //    username = Console.ReadLine();
            //}
            //Console.WriteLine("Insert password:");
            //var password = Console.ReadLine();

            //var authToken = username + ":" + password;
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(authToken);

            //Console.WriteLine("Logging in....");
            //Console.WriteLine("Logged in as: " + ParseAuthorizationHeader(Convert.ToBase64String(plainTextBytes)).Password);

            //Console.ReadLine();
            Console.WriteLine(EncodeHashToBase64("Alex" , Generatehash("Alex" , "123" , "mdk")));
            Console.ReadLine();
        }
        private static string Generatehash(string username, string password, string realm)
        {
            var hash = String.Format(
                    "{0}:{1}:{2}" ,
                    username ,
                    password ,
                    realm).ToMd5Hash();
            return hash;
        }

        private static string EncodeHashToBase64(string username, string hash)
        {
            string authstring = username + ":" +  hash;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(authstring);
            return System.Convert.ToBase64String(plainTextBytes);

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
