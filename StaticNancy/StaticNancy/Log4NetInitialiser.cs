using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;

namespace StaticNancy
{
    class Log4NetInitialiser
    {
        public void SetConfigurationFromAppConfig(string filename)
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            var file = new FileInfo(filepath);

            if (file.Exists)
                XmlConfigurator.ConfigureAndWatch(file);
            else
                Console.WriteLine("Cannot find config file: {0}", file.FullName);
        }
    }
}
