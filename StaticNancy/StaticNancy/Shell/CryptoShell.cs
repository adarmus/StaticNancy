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
                Console.WriteLine("Enter the letter:");
                string letter = Console.ReadLine();

                if (string.IsNullOrEmpty(letter))
                    break;

                Console.WriteLine();
                Console.WriteLine("Enter the text:");

                line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                var config = ConfigReader.GetConfigurationSection<NancyServiceConfigurationSection>(NancyServiceConfigurationSection.CONFIG_SECTION);

                var drive = config.Drives.FirstOrDefault(d => string.Equals(d.Letter, letter, StringComparison.CurrentCultureIgnoreCase));

                var cipher = new Cipher();
                string enc = cipher.EncryptUsingPassword(line, drive.DrivePwd);

                Console.WriteLine(enc);

            } while (!string.IsNullOrEmpty(line));
        }

        public void Decrypt()
        {
            string line;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Enter the letter:");
                string letter = Console.ReadLine();

                if (string.IsNullOrEmpty(letter))
                    break;

                Console.WriteLine();
                Console.WriteLine("Enter the crypto-text:");

                line = Console.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                var config = ConfigReader.GetConfigurationSection<NancyServiceConfigurationSection>(NancyServiceConfigurationSection.CONFIG_SECTION);

                var drive = config.Drives.FirstOrDefault(d => string.Equals(d.Letter, letter, StringComparison.CurrentCultureIgnoreCase));

                var cipher = new Cipher();
                string enc = cipher.DecryptUsingPassword(line, drive.DrivePwd);

                Console.WriteLine(enc);

            } while (!string.IsNullOrEmpty(line));
        }
    }
}
