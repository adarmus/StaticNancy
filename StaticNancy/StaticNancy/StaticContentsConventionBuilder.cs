using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Responses;

namespace StaticNancy
{
    public class StaticContentsConventionBuilder
    {
        /// <summary>
        /// Creates a delegate that maps a request for a given requestedPath to embedded resources in the given assembly.
        /// </summary>
        /// <param name="requestedPath"></param>
        /// <param name="assembly"></param>
        /// <param name="namespacePrefix"></param>
        /// <returns></returns>
        public static Func<NancyContext, string, Response> AddDirectory(string requestedPath, Assembly assembly, string namespacePrefix)
        {
            return (context, _) =>
            {
                var path = context.Request.Path;

                Console.WriteLine(path);

                if (!path.StartsWith(requestedPath))
                {
                    return null;
                }

                string resourcePath;
                string name;

                var adjustedPath = path.Substring(requestedPath.Length + 1);
                if (adjustedPath.IndexOf('/') >= 0)
                {
                    name = Path.GetFileName(adjustedPath);
                    resourcePath = namespacePrefix + "." + adjustedPath.Substring(0, adjustedPath.Length - name.Length - 1).Replace('/', '.');
                }
                else
                {
                    name = adjustedPath;
                    resourcePath = namespacePrefix;
                }

                return new EmbeddedFileResponse(assembly, resourcePath, name);
            };
        }
    }
}
