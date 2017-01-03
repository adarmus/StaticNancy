using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Responses;
using Nancy.Conventions;
using StaticNancy.Content;

namespace StaticNancy
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add(StaticContentsConventionBuilder.AddDirectory("/Files", Assembly.GetAssembly(typeof(DummyClass)), "StaticNancy.Content.Files"));
        }
    }
}
