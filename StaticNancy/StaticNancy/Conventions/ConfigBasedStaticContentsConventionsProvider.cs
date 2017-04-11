using System;
using System.Collections.Generic;
using Nancy;
using StaticNancy.Config;
using StaticNancy.Logging;
using System.Linq;

namespace StaticNancy.Conventions
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

            var builder = new StaticContentsConventionBuilder(_log);

            foreach (var resource in _config.ResourceProviders.Where(p => p.Enabled))
            {
                var ass = _types.GetAssembly(resource.AssemblyName);

                if (ass == null)
                {
                    _log.WriteLineDebug("Provider {0}: Cannot load assembly: {1}", resource.Name, resource.AssemblyName);
                }
                else
                {
                    _log.WriteLineDebug("Provider {0}: path: {1}; prefix: {2}; assembly: {3}", resource.Name, resource.RequestRootPath, resource.ResourcePrefix, resource.AssemblyName);

                    list.Add(builder.AddDirectory(resource.RequestRootPath, ass, resource.ResourcePrefix, resource.DefaultResource));
                }
            }

            return list;
        }
    }
}
