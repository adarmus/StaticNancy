using System;
using System.Reflection;

namespace StaticNancy.Shell
{
    class MenuShell
    {
        internal void Start()
        {
            Console.WriteLine("Nancy Server v{0}", Assembly.GetCallingAssembly().GetName().Version);

            bool showMenu = true;
            ConsoleKeyInfo key;

            do
            {
                if (showMenu)
                {
                    Console.WriteLine();
                    Console.WriteLine("Select one of the following options:");
                    Console.WriteLine("    1 - Start the service");
                    Console.WriteLine("    2 - Install Windows Service");
                    Console.WriteLine("    3 - Uninstall Windows Service");
                    Console.WriteLine("Press [ESCAPE] to close");
                    Console.WriteLine();
                }

                key = Console.ReadKey(true);
                showMenu = true;
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        StartInterface();
                        break;
                    case ConsoleKey.D2:
                        Install();
                        break;
                    case ConsoleKey.D3:
                        UnInstall();
                        break;
                    case ConsoleKey.D4:
                        Encrypt();
                        break;
                    default:
                        showMenu = false;
                        break;
                }
            } while (key.Key != ConsoleKey.Escape);
        }

        void Encrypt()
        {
            var console = new CryptoShell();
            console.Encrypt();
        }

        void Install()
        {
            var console = new InstallerShell();
            console.Install();
        }

        void UnInstall()
        {
            var console = new InstallerShell();
            console.UnInstall();
        }

        void StartInterface()
        {
            var shell = new ConsoleShell();
            shell.Start();
        }
    }
}
