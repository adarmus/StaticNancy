using System;
using System.ServiceProcess;

namespace StaticNancy.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
            {
                var servicesToRun = new ServiceBase[] { new ServiceShell() };
                ServiceBase.Run(servicesToRun);
            }
            else
            {
                var shell = new MenuShell();
                shell.Start();
            }
        }
    }
}
