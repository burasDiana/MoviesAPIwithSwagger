using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TestWebAPI.Filters
{
    /// <summary>
    /// Validates the model properties according to the rules set in the models
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var nullObj = actionContext.ActionArguments.Where(kv => kv.Value == null)
                .Select(kv => $"The argument '{kv.Key}' cannot be null");

            if (nullObj.Any())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    (HttpStatusCode) 422, string.Join("\n", nullObj));
            }

            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    (HttpStatusCode) 422, actionContext.ModelState);
            }

        }
    }
}