using System.Collections.Generic;
using System.Dynamic;

namespace WorkflowActionSdk.Extentions
{
    public static class ExpandoObjectExtensions
    {
        public static object GetField(this ExpandoObject expandoObject, string path)
        {
            if (expandoObject == null)
                return null;

            var pathSegments = path.Split('.');
            var lastPathSegment = pathSegments[pathSegments.Length - 1];

            var cursor = (IDictionary<string, object>)expandoObject;

            for (int i = 0; i < pathSegments.Length - 1; i++)
            {
                var pathSegment = pathSegments[i];

                cursor = (IDictionary<string, object>)cursor[pathSegment];
            }

            object result = null;

            cursor.TryGetValue(lastPathSegment, out result);

            return result;
        }       
    }
}
