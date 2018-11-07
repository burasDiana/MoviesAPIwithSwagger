using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace TestWebAPI.Security
{
    public class ApplyBasicAuth : IOperationFilter
    {
        public string Name { get; private set; }

        public ApplyBasicAuth()
        {
            Name = "basic";
        }

        public void Apply(Operation operation , SchemaRegistry schemaRegistry , ApiDescription apiDescription)
        {
            var basicAuthDict = new Dictionary<string , IEnumerable<string>>();
            basicAuthDict.Add(Name , new List<string>());
            operation.security = new IDictionary<string , IEnumerable<string>>[] { basicAuthDict };

            if ( operation.parameters == null )
                operation.parameters = new List<Parameter>();

            //add another header in swagger
            operation.parameters.Add(new Parameter
            {
                name = "Accept-Version" ,
                @in = "header" ,
                type = "string" ,
                description = "Client-Version" ,
                required = true
            });

            //add token param
            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header", //this property must be set to header to indicate how swagger interprets it
                type = "string",
                description = "Authentication scheme = 'Token'",
                required = false
            });
        }
    }
}