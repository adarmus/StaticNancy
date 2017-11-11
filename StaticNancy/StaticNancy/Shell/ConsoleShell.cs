using System;
using System.Reflection;
using StaticNancy.Logging;

namespace StaticNancy.Shell
{
    class ConsoleShell
    {
        ITraceLogger _log;
        Server _server;

        public void Start()
        {
            new Log4NetInitialiser("Log4net.config").SetConfigurationFromFile();

            _server = null;
            _log = new TraceLogger("Console");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _log.WriteLineInfo("Nancy Server v{0}", Assembly.GetCallingAssembly().GetName().Version);

            _server = new Server(_log);

            _server.Start();

            _log.WriteLineInfo("Press [ESCAPE] to shutdown server");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
            }

            _server.Stop();

            Environment.Exit(0);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (ex != null)
                UnhandledException(ex);
        }

        void UnhandledException(Exception ex)
        {
            _log.WriteLineInfo(ex.GetFullMessage());
            _log.WriteLineInfo(ex.StackTrace);
            _log.WriteLineInfo("\nCrunch, press [ESCAPE] to shutdown server");

            try
            {
                while (Console.ReadKey(true).Key != ConsoleKey.Escape)
                {
                }
            }
            finally
            {
                Environment.Exit(1);
            }
        }
    }
}
