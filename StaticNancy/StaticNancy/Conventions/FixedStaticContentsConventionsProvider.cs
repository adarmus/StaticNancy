using System;
using System.Collections.Generic;
using System.Reflection;
using Nancy;
using StaticNancy.Logging;

namespace StaticNancy.Conventions
{
    class FixedStaticContentsConventionsProvider : IStaticContentsConventionsProvider
    {
        public IEnumerable<Func<NancyContext, string, Response>> GetConventions()
        {
            var log = new TraceLogger("Fixed");
            var builder = new StaticContentsConventionBuilder(log);
            return new []
            {
                builder.AddDirectory("/Files", Assembly.GetAssembly(typeof (FixedStaticContentsConventionsProvider)), "StaticNancy.Content.Files")
            };
        }
    }
}
