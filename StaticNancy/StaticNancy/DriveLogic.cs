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
            _log = new TraceLogger("DriveLogic");

            //Test("\"aaa bbb\" cc dd");
            //Test("aaa bbb cc dd");
            //Test("aaa");
        }

        void Test(string input)
        {
            string file;
            string args;
            GetCommandArgs(input, out file, out args);
        }

        DriveElement GetDriveConfig(string letter)
        {
            var drive = _config.Drives.FirstOrDefault(d => string.Equals(d.Letter, letter, StringComparison.CurrentCultureIgnoreCase));
            return drive;
        }

        public void DoMount(string letter)
        {
            var drive = GetDriveConfig(letter);

            if (drive == null)
                return;

            if (string.IsNullOrEmpty(drive.DriveMountCommand))
                return;

            RunCommand(drive.DriveMountCommand, drive.DrivePwd);
        }

        public void DoUnmount(string letter)
        {
            var drive = GetDriveConfig(letter);

            if (drive == null)
                return;

            if (string.IsNullOrEmpty(drive.DriveUnmountCommand))
                return;

            RunCommand(drive.DriveUnmountCommand, drive.DrivePwd);
        }

        void RunCommand(string encCommand, string pwd)
        {
            try
            {
                var cipher = new Cipher();
                string command = cipher.DecryptUsingPassword(encCommand, pwd);
                _log.WriteLineDebug("CMD {0}", command);

                string file;
                string args;
                GetCommandArgs(command, out file, out args);

                var p = Process.Start(file, args);
            }
            catch (Exception ex)
            {
                _log.WriteLineError(ex, "Cmd: {0}", encCommand);
                throw;
            }
        }

        void GetCommandArgs(string fullCommand, out string file, out string args)
        {
            file = string.Empty;
            args = string.Empty;

            if (string.IsNullOrEmpty(fullCommand))
                return;

            if (fullCommand.StartsWith("\""))
            {
                // "<filepath>" <args>
                // Split on the 2nd "
                int i = fullCommand.IndexOf("\"", 1);
                file = fullCommand.Substring(0, i + 1);
                args = fullCommand.Substring(i + 2);
            }
            else if (fullCommand.Contains(" "))
            {
                // <filepath> args
                // Split on the 1st space
                int i = fullCommand.IndexOf(" ");
                file = fullCommand.Substring(0, i);
                args = fullCommand.Substring(i + 1);
            }
            else
            {
                // <filepath>
                // No args - just a filepath
                file = fullCommand;
            }
        }

        public DriveInfo GetDrive(string driveLetter)
        {
            string letter = driveLetter.Substring(0, 1);

            var drive = GetDriveConfig(letter);

            if (drive == null)
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
