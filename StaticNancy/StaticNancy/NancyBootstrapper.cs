using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Nancy;
using Nancy.Conventions;
using StaticNancy.Content;

namespace StaticNancy
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        readonly IStaticContentsConventionsProvider _conventions;

        public NancyBootstrapper()
        {
            _conventions = new FixedStaticContentsConventionsProvider();
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            foreach (var convention in _conventions.GetConventions())
            {
                nancyConventions.StaticContentsConventions.Add(convention);
            }
        }
    }
}
