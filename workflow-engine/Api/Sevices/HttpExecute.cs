using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;

namespace Api.Sevices
{
    public class HttpExecute : HttpMethodAttribute
    {
        private static readonly IEnumerable<string> _supportedMethods = new[] { "EXECUTE" };
        public HttpExecute()
        : base(_supportedMethods)
        {
        }

        public HttpExecute(string template)
            : base(_supportedMethods, template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
        }
    }
}
