using StaticNancy.Config;
using StaticNancy.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Shell
{
    class CryptoShell
    {
        public void Encrypt()
        {
            string line;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Enter the text:");

                line = Console.ReadLine();

                var config = ConfigReader.GetConfigurationSection<NancyServiceConfigurationSection>(NancyServiceConfigurationSection.CONFIG_SECTION);

                var cipher = new Cipher();
                string enc = cipher.EncryptUsingPassword(line, config.DrivePwd);

                Console.WriteLine(enc);

            } while (!string.IsNullOrEmpty(line));
        }
    }
}
