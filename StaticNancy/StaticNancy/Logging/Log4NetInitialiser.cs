using System;
using System.IO;
using log4net.Config;

namespace StaticNancy.Logging
{
    public class Log4NetInitialiser
    {
        readonly string _filename;

        public Log4NetInitialiser(string filename)
        {
            _filename = filename;
        }

        public void SetConfigurationFromFile()
        {
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _filename);
            var file = new FileInfo(filepath);

            if (file.Exists)
                XmlConfigurator.ConfigureAndWatch(file);
            else
                Console.WriteLine("Cannot find config file: {0}", file.FullName);
        }
    }
}
