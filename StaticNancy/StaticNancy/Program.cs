using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;

namespace StaticNancy
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
