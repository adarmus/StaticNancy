using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy
{
    public class RootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            var b = AppDomain.CurrentDomain.BaseDirectory;
            return b;
        }
    }
}
