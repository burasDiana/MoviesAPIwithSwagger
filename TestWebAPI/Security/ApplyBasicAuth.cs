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
        }
    }
}