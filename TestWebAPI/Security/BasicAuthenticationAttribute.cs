using System;
using System.Collections.Generic;
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
        //client sents credidentials in the authentication header
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(actionContext.Request.Headers.Authorization == null )
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
            }
            else //if header exists, retrieve username and password
            {
                #region basic auth implementation
                // FORMAT=> username:password in base 64
                //string authToken = actionContext.Request.Headers.Authorization.Parameter;
                //string decodedAuthToken = Encoding.UTF8.GetString( Convert.FromBase64String(authToken));
                //string[] upArray = decodedAuthToken.Split(':');
                //string username = upArray[0];
                //string password = upArray[1];

                //if ( UserSecurity.Login(username , password) )
                //{
                //    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                //}
                //else
                //{
                //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
                //}
                #endregion
                //authtoken format => username:randomkey:hash
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string[]  upArray = decodedAuthToken.Split(new[] { ':' });
                string username = upArray[0];
                string key = upArray[1];
                string password = UserSecurity.GetPasswordForUser(username);
                string encodedbytes = upArray[2];
                string domain = "mdk";

                var hash = String.Format(
                    "{0}:{1}:{2}:{3}" ,
                    username,
                    password,
                    key,
                    domain).ToMd5Hash();

                if(encodedbytes.Equals(hash, StringComparison.Ordinal) )
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username) , null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized , "Unauthorized request");
                }

            }
        }
    }
}