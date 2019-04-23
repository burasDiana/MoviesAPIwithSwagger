using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TestWebAPI.Security
{
    public class VersionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if ( actionContext.Request.Headers.Contains("Accept-Version") )
            {
                string version = actionContext.Request.Headers.GetValues("Accept-Version").First();
                if( version != "3.0" )
                {
                    actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.PreconditionFailed , "Version too old, please upgrade to newest version");
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized , "Version code missing");
            }
        }

      
    }
}