using StaticNancy.Config;
using StaticNancy.Crypto;
using StaticNancy.Logging;
using StaticNancy.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy
{
    class DriveLogic
    {
        readonly ITraceLogger _log;
        readonly NancyServiceConfigurationSection _config;

        public DriveLogic(NancyServiceConfigurationSection config)
        {
            _config = config;
        }

        public void DoMount(string letter)
        {
            if (string.IsNullOrEmpty(_config.DriveMountCommand))
                return;

            RunCommand(_config.DriveMountCommand);
        }

        public void DoUnmount(string letter)
        {
            if (string.IsNullOrEmpty(_config.DriveUnmountCommand))
                return;

            RunCommand(_config.DriveUnmountCommand);
        }

        void RunCommand(string encCommand)
        {
            try
            {
                var cipher = new Cipher();
                string command = cipher.DecryptUsingPassword(encCommand, _config.DrivePwd);
                _log.WriteLineDebug("CMD {0}", command);

                var p = Process.Start(command);
            }
            catch (Exception ex)
            {
                _log.WriteLineError(ex, "Cmd: {0}", encCommand);
                throw;
            }
        }

        public DriveInfo GetDrive(string driveLetter)
        {
            string letter = driveLetter.Substring(0, 1);

            if (!string.Equals(letter, _config.Drive, StringComparison.CurrentCultureIgnoreCase))
            {
                return new DriveInfo
                {
                    Drive = letter,
                    Exists = false,
                    CanMount = false
                };
            }

            bool exists = DoesDriveExist(letter);

            return new DriveInfo
            {
                Drive = letter,
                Exists = exists,
                CanMount = true
            };
        }

        bool DoesDriveExist(string letter)
        {
            var drives = System.IO.Directory.GetLogicalDrives();

            var root = System.IO.Directory.GetDirectoryRoot($"{letter}:");

            bool exists = drives.Contains(root);
            return exists;
        }
    }
}
