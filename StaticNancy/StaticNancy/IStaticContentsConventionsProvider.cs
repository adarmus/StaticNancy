using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace StaticNancy
{
    interface IStaticContentsConventionsProvider
    {
        IEnumerable<Func<NancyContext, string, Response>> GetConventions();
    }
}
