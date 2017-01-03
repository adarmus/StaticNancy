using System;
using System.IO;
using log4net.Config;

namespace StaticNancy.Logging
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
