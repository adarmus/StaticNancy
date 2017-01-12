using System;
using System.Collections.Generic;
using Nancy;

namespace StaticNancy.Conventions
{
    interface IStaticContentsConventionsProvider
    {
        IEnumerable<Func<NancyContext, string, Response>> GetConventions();
    }
}
