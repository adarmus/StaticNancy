using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nancy;
using Nancy.Conventions;
using StaticNancy.Config;
using StaticNancy.Conventions;
using StaticNancy.Logging;

namespace StaticNancy
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        readonly ITraceLogger _log;

        public NancyBootstrapper()
        {
            _log = new TraceLogger("Bootstrapper");
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            try
            {
                AddStaticConventions(nancyConventions);
            }
            catch (Exception ex)
            {
                _log.WriteLineError("An error occured configuring conventions", ex);
            }
        }

        void AddStaticConventions(NancyConventions nancyConventions)
        {
            Func<NancyContext, string, Response>[] conventions = GetConventions();

            _log.WriteLineDebug("Adding {0} conventions", conventions.Length);

            foreach (var convention in conventions)
            {
                nancyConventions.StaticContentsConventions.Add(convention);
            }
        }

        Func<NancyContext, string, Response>[] GetConventions()
        {
            var conventions = GetStaticContentsConventionsProvider()
                .GetConventions()
                .ToArray();

            return conventions;
        }

        IStaticContentsConventionsProvider GetStaticContentsConventionsProvider()
        {
            //return new FixedStaticContentsConventionsProvider();

            var config = ConfigReader.GetConfigurationSection<NancyServiceConfigurationSection>(NancyServiceConfigurationSection.CONFIG_SECTION);
            return new ConfigBasedStaticContentsConventionsProvider(_log, config);
        }
    }
}
