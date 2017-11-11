using System;
using System.Reflection;
using System.ServiceProcess;
using StaticNancy.Logging;

namespace StaticNancy.Shell
{
    class ServiceShell : ServiceBase
    {
        readonly ITraceLogger _log;
        Server _server;

        public ServiceShell()
        {
            new Log4NetInitialiser("Log4net.config").SetConfigurationFromFile();

            _server = null;
            _log = new TraceLogger("Service");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            this.ServiceName = "Nancy Server";

            _log.WriteLineDebug("{0} v{1}", this.ServiceName, Assembly.GetCallingAssembly().GetName().Version);

            InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {
            _server = new Server(_log);

            try
            {
                _server.Start();
            }
            catch (Exception ex)
            {
                _log.WriteLineInfo("An error occured during startup");
                _log.WriteLineInfo(ex.GetFullMessage());
                _log.WriteLineInfo(ex.StackTrace);

                throw;
            }
        }

        protected override void OnStop()
        {
            if (_server != null)
                _server.Stop();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (ex != null)
                UnhandledException(ex);
        }

        void UnhandledException(Exception ex)
        {
            _log.WriteLineInfo("An unhandled error occured");
            _log.WriteLineInfo(ex.Message);
            _log.WriteLineInfo(ex.StackTrace);

            Environment.Exit(1);
        }

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "ServiceShell";
        }
    }
}
