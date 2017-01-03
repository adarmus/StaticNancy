using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nancy;
using Nancy.Conventions;
using StaticNancy.Content;
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

            var conventions = GetStaticContentsConventionsProvider()
                .GetConventions()
                .ToArray();

            _log.WriteLineDebug("Adding {0} conventions", conventions.Length);

            foreach (var convention in conventions)
            {
                nancyConventions.StaticContentsConventions.Add(convention);
            }
        }

        IStaticContentsConventionsProvider GetStaticContentsConventionsProvider()
        {
            return new FixedStaticContentsConventionsProvider();
        }
    }
}
