using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using StaticNancy.Logging;

namespace StaticNancy
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
