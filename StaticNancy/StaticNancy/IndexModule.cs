using Nancy;
using StaticNancy.Config;
using StaticNancy.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StaticNancy
{
    internal class IndexModule : NancyModule
    {
        readonly ITraceLogger _log;
        readonly NancyServiceConfigurationSection _config;

        public IndexModule()
            : base("/")
        {
            _log = new TraceLogger("IndexModule");
            _config = ConfigReader.GetConfigurationSection<NancyServiceConfigurationSection>(NancyServiceConfigurationSection.CONFIG_SECTION);

            this.Get["/Index", runAsync: true] = this.OnIndex;
        }

        private Task<object> OnIndex(object parameters, CancellationToken token)
        {
            _log.WriteLineDebug("Index: {0}", parameters);

            return Task.FromResult<object>(View["Views/Index.sshtml"]);
        }

        void AddIndexEndpoint(NancyServiceConfigurationSection config)
        {
            foreach (var resource in config.ResourceProviders)
            {
                _log.WriteLineDebug("Provider {0}: path: {1}; prefix: {2}; assembly: {3}", resource.Name, resource.RequestRootPath, resource.ResourcePrefix, resource.AssemblyName);

            }
        }
    }
}
