using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;

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

            _host = new NancyHost(new Uri("http://localhost:12345"));
            _host.Start();
            _log.WriteLineInfo("Running on http://localhost:12345");

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
