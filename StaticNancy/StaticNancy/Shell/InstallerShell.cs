using System;
using System.Configuration.Install;
using System.Reflection;

namespace StaticNancy.Shell
{
    class InstallerShell
    {
        public void Install()
        {
            DoIt(new[] { Assembly.GetExecutingAssembly().Location });
        }

        public void UnInstall()
        {
            DoIt(new[] { "/u", Assembly.GetExecutingAssembly().Location });
        }

        void DoIt(string[] args)
        {
            try
            {
                ManagedInstallerClass.InstallHelper(args);
            }
            catch (Exception ex)
            {
                string full = ex.GetFullMessage();
                Console.WriteLine();
                Console.WriteLine("An error occured");
                Console.WriteLine(full);
            }
        }
    }
}
