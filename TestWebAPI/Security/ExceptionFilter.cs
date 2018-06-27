﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace TestWebAPI.Security
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string exceptionMessage = string.Empty;
            string exceptionContent = string.Empty;
            if ( actionExecutedContext.Exception.InnerException == null )
            {
                exceptionMessage = actionExecutedContext.Exception.Message;
            }
            else
            {
                exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
            }
           
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {   
                Content = new StringContent("An unhandled exception was thrown by service.") ,
                ReasonPhrase = "Internal Server Error.Please Contact your Administrator."
            };

            if ( actionExecutedContext.Exception is DivideByZeroException )
            {
                exceptionContent = "Cannot divide by zero";
                response.Content = new StringContent(exceptionContent);
            }
           
            actionExecutedContext.Response = response;
        }
    }
}