using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
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

            //create rolestringbuilder
            var roleRequirementBuilder = new StringBuilder();

            //get the current operation description
            var originalDescription = operation.description;

            //create filter handler
            var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline().Where(f => f.Scope == System.Web.Http.Filters.FilterScope.Action);

            //get roles object via filter
            var roles = filterPipeline.Select(f => f.Instance).OfType<CustomAuthenticationAttribute>()
                .Select(a => a.UserTypes).FirstOrDefault();

            roleRequirementBuilder.AppendLine(originalDescription);
            roleRequirementBuilder.Append("<br/>");

            if (roles == null)
            {
                roleRequirementBuilder.AppendLine("**Roles Required: ** None");
            }
            else
            {
                bool isAdminPresent = false;
                roleRequirementBuilder.AppendLine("**Roles Required**");

                if (roles.Contains(UserSecurity.UserType.Admin))
                {
                    roleRequirementBuilder.AppendLine("Admin");
                    isAdminPresent = true;
                }

                if (roles.Contains(UserSecurity.UserType.Customer))
                {
                    if (isAdminPresent)
                    {
                        roleRequirementBuilder.AppendLine(", Customer");
                    }
                    else
                    {
                        roleRequirementBuilder.AppendLine("Customer");
                    }
                }
            }

            //add another header in swagger
            //operation.parameters.Add(new Parameter
            //{
            //    name = "Accept-Version" ,
            //    @in = "header" ,
            //    type = "string" ,
            //    description = "Client-Version" ,
            //    required = true,
            //    @default = "3.0"
            //});

            //add token param
            //operation.parameters.Add(new Parameter
            //{
            //    name = "Authorization",
            //    @in = "header", //this property must be set to header to indicate how swagger interprets it
            //    type = "string",
            //    description = "Authentication scheme = 'Token'",
            //    required = false,
            //    @default = "Token zSdelNJxbaVun3qpiJCQ",
            //});

            //add the roles in the description
            operation.description = roleRequirementBuilder.ToString();
        }
    }
}