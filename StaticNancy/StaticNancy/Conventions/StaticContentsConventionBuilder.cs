using System;
using System.IO;
using System.Reflection;
using Nancy;
using Nancy.Responses;
using StaticNancy.Logging;

namespace StaticNancy.Conventions
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
            return AddDirectory(requestedPath, assembly, namespacePrefix, null);
        }

        /// <summary>
        /// Creates a delegate that maps a request for a given requestedPath to embedded resources in the given assembly.
        /// </summary>
        /// <param name="requestRootPath"></param>
        /// <param name="assembly"></param>
        /// <param name="namespacePrefix"></param>
        /// <param name="defaultResource"></param>
        /// <returns></returns>
        public Func<NancyContext, string, Response> AddDirectory(string requestRootPath, Assembly assembly, string namespacePrefix, string defaultResource)
        {
            return (context, _) =>
            {
                var path = context.Request.Path;

                _log.WriteLineDebug("Request {0}; root={1}; prefix={2}; assembly={3}", path, requestRootPath, namespacePrefix, assembly.GetName());

                if (!path.StartsWith(requestRootPath))
                {
                    _log.WriteLineDebug("Response null {0}", path);
                    return null;
                }

                var relativePath = path.Length == requestRootPath.Length ? string.Empty : path.Substring(requestRootPath.Length + 1);

                if (string.IsNullOrEmpty(relativePath) && !string.IsNullOrEmpty(defaultResource))
                {
                    relativePath = defaultResource;
                }

                string resourcePath;
                string name;

                if (relativePath.IndexOf('/') >= 0)
                {
                    name = Path.GetFileName(relativePath);
                    resourcePath = namespacePrefix + "." + relativePath.Substring(0, relativePath.Length - name.Length - 1).Replace('/', '.');
                }
                else
                {
                    name = relativePath;
                    resourcePath = namespacePrefix;
                }

                _log.WriteLineDebug("Response {0}; path={1}; name={2}", path, resourcePath, name);

                return new EmbeddedFileResponse(assembly, resourcePath, name);
            };
        }
    }
}
