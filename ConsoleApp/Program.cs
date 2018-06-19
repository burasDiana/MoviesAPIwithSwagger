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
            Console.WriteLine("Insert username:");
            var username = Console.ReadLine();
            while (!CheckUsername(username) )
            {
                Console.WriteLine("Username must be a valid email address:");
                username = Console.ReadLine();
            }
            Console.WriteLine("Insert password:");
            var password = Console.ReadLine();

            var authToken = username + ":" + password;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(authToken);

            Console.WriteLine("Logging in....");
            Console.WriteLine("Logged in as: " + ParseAuthorizationHeader(Convert.ToBase64String(plainTextBytes)).Password);

            Console.ReadLine();
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
