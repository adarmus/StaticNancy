using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;
using StaticNancy.Config;
using StaticNancy.Logging;

namespace StaticNancy
{
    class Server
    {
        readonly ITraceLogger _log;
        NancyHost _host;

        public Server(ITraceLogger log)
        {
            _log = log;
        }

        public void Start()
        {
            _log.WriteLineInfo("Starting server...");

            var config = ConfigReader.GetConfigurationSection<NancyServiceConfigurationSection>(NancyServiceConfigurationSection.CONFIG_SECTION);

            string url = string.Format("http://localhost:{0}/", config.Port);

            _host = new NancyHost(new Uri(url));
            _host.Start();
            _log.WriteLineInfo("Running on {0}", url);

            _log.WriteLineInfo("Starting server...done");
        }

        public void Stop()
        {
            _log.WriteLineInfo("Stopping server...");

            if (_host != null)
                _host.Dispose();

            _log.WriteLineInfo("Stopping server...done");
        }
    }
}
