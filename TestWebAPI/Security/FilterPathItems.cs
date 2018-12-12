using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace TestWebAPI.Security
{
    /// <summary>
    /// Class used for filtering only specific operation parameters for each path in swagger UI
    /// </summary>
    public class FilterPathItems : IDocumentFilter
    {
        private string tokenUrlRoute = "token";

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            //get the token path operation
            var tokenPathItem = swaggerDoc.paths
                .Where(entry => entry.Key.Contains(tokenUrlRoute))
                .FirstOrDefault().Value;

            AddHeadersToPathObject(tokenPathItem);

            //get the other paths which expose API resources
            var allotherPaths = swaggerDoc.paths.Where(entry => !entry.Key.Contains(tokenUrlRoute))
                .Select(entry => entry.Value)
                .ToList();

            if (allotherPaths != null)
            {
                foreach (var path in allotherPaths)
                {
                    if (path.parameters == null)
                    {
                        path.parameters = new List<Parameter>();

                        path.parameters.Add(
                            new Parameter
                            {
                                name = "Authorization",
                                @in = "header",
                                type = "string",
                                description = "Token Auth.",
                                required = true,
                                @default = "Token zSdelNJxbaVun3qpiJCQ"
                            }
                        );
                    }
                }
            }
        }

        /// <summary>
        /// This method creates a new 'Accept-Version' parameter object description for the path item passed in
        /// the parameter represents a header's properties
        /// </summary>
        /// <param name="tokenPathItem"></param>
        private void AddHeadersToPathObject(PathItem tokenPathItem)
        {
            if (tokenPathItem != null)
            {
                if (tokenPathItem.parameters != null)
                {
                    tokenPathItem.parameters.Clear();
                }
                else
                {
                    tokenPathItem.parameters = new List<Parameter>();
                }

                tokenPathItem.parameters.Add(new Parameter
                {
                    name = "Accept-Version",
                    @in = "header",
                    type = "string",
                    description = "Client-Version",
                    required = true,
                    @enum = new List<object>() { "3.0","2.9","2.8"}
                });
            }
        }
    }
}