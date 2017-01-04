using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Responses;
using StaticNancy.Logging;

namespace StaticNancy
{
    class StaticContentsConventionBuilder
    {
        readonly ITraceLogger _log;

        public StaticContentsConventionBuilder(ITraceLogger log)
        {
            _log = log;
        }

        /// <summary>
        /// Creates a delegate that maps a request for a given requestedPath to embedded resources in the given assembly.
        /// </summary>
        /// <param name="requestedPath"></param>
        /// <param name="assembly"></param>
        /// <param name="namespacePrefix"></param>
        /// <returns></returns>
        public Func<NancyContext, string, Response> AddDirectory(string requestedPath, Assembly assembly, string namespacePrefix)
        {
            return (context, _) =>
            {
                var path = context.Request.Path;

                //Console.WriteLine(path);

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

                _log.WriteLineDebug("Request {0}; path={1}; name={2}", path, resourcePath, name);

                return new EmbeddedFileResponse(assembly, resourcePath, name);
            };
        }
    }
}
