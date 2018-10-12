using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Security.Principal;

namespace TestWebAPI.Security
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public UserSecurity.UserType[] UserTypes { get; set; }

        public BasicAuthenticationAttribute(params UserSecurity.UserType[] _userTypes)
        {
            UserTypes = _userTypes;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            
            if (actionContext.RequestContext.RouteData.Route.RouteTemplate.Contains("token")) // can also use Headers.Authorization.scheme =Token or Basic
            {
                //authenticate if auth header is present
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized request, no authorization header provided");
                }
                else //if header exists, retrieve username and password
                {
                    #region basic auth implementation
                    string authToken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodedAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                    string[] upArray = decodedAuthToken.Split(':');
                    string username = upArray[0];
                    string password = upArray[1];

                    if (UserSecurity.Login(username, password))
                    {
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                        
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid credentials");
                    }
                    #endregion
                }

                #region authtoken with MD5 hash implementation
                //private static List<string> knowncodesforAlex = new List<string>() {"2LO1AASEOXUQP5LJ8KOFYW5"};
                //authtoken format => username:randomkey:hash
                //string authToken = actionContext.Request.Headers.Authorization.Parameter;
                //string decodedAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                //string[]  upArray = decodedAuthToken.Split(new[] { ':' });

                //if ( upArray.Count() != 3 )
                //{
                //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized , "Authorization header format incorrect, please send in the following format  username:key:hash ");
                //    return;
                //}


                //string username = upArray[0];
                //string key = upArray[1];
                //string password = UserSecurity.GetPasswordForUser(username);
                //string encodedbytes = upArray[2];
                //string domain = "mdk";

                //if ( knowncodesforAlex.Contains(key) )
                //{
                //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized , "Unique key already exists. Please generate a new key according to the agreed method.");
                //    return;
                //}
                //else
                //{
                //    knowncodesforAlex.Add(key);
                //}

                //var hash = String.Format(
                //    "{0}:{1}:{2}:{3}" ,
                //    username,
                //    password,
                //    key,
                //    domain).ToMd5Hash();

                //if(encodedbytes.Equals(hash, StringComparison.Ordinal) )
                //{
                //    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username) , null);
                //}
                //else
                //{
                //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized , "Unauthorized request, password did not match");
                //}
                #endregion

            }
            else
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized request, no authorization header provided");
                }
                else
                {
                    string authToken = actionContext.Request.Headers.Authorization.Parameter;
                    if (Token.TokenExists(authToken))
                    {
                        int userId =  Token.GetUserID(authToken);
                        string userName = UserSecurity.GetUserName(userId);
                        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                        if (!UserRole_Allowed(UserSecurity.UserType.Customer))
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, "You do not have access to this function!");
                        }
                    }
                    else 
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized request, invalid or expired token");
                    }

                }
            }
        }

        private bool UserRole_Allowed(UserSecurity.UserType role)
        {
            if (UserTypes.Contains(role))
                return true;
            return false;
        }
    }
}