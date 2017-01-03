using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using StaticNancy.Config;
using StaticNancy.Logging;

namespace StaticNancy
{
    class ConfigBasedStaticContentsConventionsProvider : IStaticContentsConventionsProvider
    {
        readonly ITraceLogger _log;
        readonly NancyServiceConfigurationSection _config;
        readonly TypeLoader _types;

        public ConfigBasedStaticContentsConventionsProvider(ITraceLogger log, NancyServiceConfigurationSection config)
        {
            _log = log;
            _config = config;
            _types = new TypeLoader();
        }

        public IEnumerable<Func<NancyContext, string, Response>> GetConventions()
        {
            var list = new List<Func<NancyContext, string, Response>>();

            _log.WriteLineDebug("Loading {0} resource providers", _config.ResourceProviders.Count);

            foreach (var resource in _config.ResourceProviders)
            {
                var ass = _types.GetAssembly(resource.AssemblyName);

                if (ass == null)
                {
                    _log.WriteLineDebug("Provider {0}: Cannot load assembly: {1}", resource.Name, resource.AssemblyName);
                }
                else
                {
                    _log.WriteLineDebug("Provider {0}: path: {1}; prefix: {2}; assembly: {3}", resource.Name, resource.RequestedPath, resource.ResourcePrefix, resource.AssemblyName);
                    list.Add(StaticContentsConventionBuilder.AddDirectory(resource.RequestedPath, ass, resource.ResourcePrefix));
                }
            }

            return list;
        }
    }
}
