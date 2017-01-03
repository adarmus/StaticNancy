using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using StaticNancy.Content;

namespace StaticNancy
{
    class FixedStaticContentsConventionsProvider : IStaticContentsConventionsProvider
    {
        public IEnumerable<Func<NancyContext, string, Response>> GetConventions()
        {
            return new []
            {
                StaticContentsConventionBuilder.AddDirectory("/Files", Assembly.GetAssembly(typeof (DummyClass)), "StaticNancy.Content.Files")
            };
        }
    }
}
