using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace TestWebAPI.Security
{
    /// <summary>
    /// Helper class for storing values or objects to be used withing the controller methods
    /// </summary>
    public class AuthHelper
    {
        public static string[] acceptedVersions = {"2.3.4", "2.3.5","3.0"}; // [2.3.4, 2.3.5, 2.3.6, 3.0] as of 04-03-2019
        public static string[] newestVersions = { "3.0" }; // [3.0] as of 04-03-2019
        public static string TokenType = "Token";
        public static string PlatformHeaderName = "Platform";
        public static string VersionHeaderName = "AppVersion";
        public static string DeviceTokenHeaderName = "DeviceToken"; // represents the device id from play/app store

        public static CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal;}
            set { HttpContext.Current.User = value; }
        }
        public static string AppVersion { get; set; }
        public static string DeviceToken { get; set; }
        public static string Platform { get; set; }
        public static bool NewVersionExists { get; set; }
        public static string TokenExpiresIn { get; set; }
        public static string TokenValue { get; set; }
    }

    /// <summary>
    /// This class holds information about the user which sends a request to the API during one session
    /// </summary>
    public class CustomPrincipal : IPrincipal
    {
        public string[] Roles { get; set; }
        public IIdentity Identity { get; private set; }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserSecurity.UserType Type { get; set; }
        public string Version { get; set; }
        public string VersionResponse { get; set; }
        public string Language { get; set; }
        public string SessionToken { get; set; }
        public string Platform { get; set; }
        public string Email { get; set; }

        public bool PasswordIsTemporary { get; set; } //used for resetting password

        public bool IsInRole(string role)
        {
            return Roles.Any(role.Contains);
        }

        public CustomPrincipal(int id, string username, string firstName, string lastName,
            UserSecurity.UserType userType)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Type = userType;
            Identity = new GenericIdentity(username);
            Email = username;
        }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }

    }

    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}