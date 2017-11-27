using Nancy;
using StaticNancy.Config;
using StaticNancy.Crypto;
using StaticNancy.Logging;
using StaticNancy.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StaticNancy
{
    public class IndexModule : NancyModule
    {
        readonly ITraceLogger _log;
        readonly NancyServiceConfigurationSection _config;

        public IndexModule()
            : base("/")
        {
            _log = new TraceLogger("IndexModule");
            _config = ConfigReader.GetConfigurationSection<NancyServiceConfigurationSection>(NancyServiceConfigurationSection.CONFIG_SECTION);

            this.Get["/Index", runAsync: true] = this.OnIndex;
            this.Get["/Drive/{drive}", runAsync: true] = this.OnDrivePage;
            this.Get["/Drive/{drive}/status", runAsync: true] = this.OnDriveStatusGet;
            this.Post["/Drive/{drive}/status", runAsync: true] = this.OnDriveStatusPost;
        }

        private Task<object> OnIndex(object parameters, CancellationToken token)
        {
            _log.WriteLineDebug("Index: {0}", parameters);

            IndexInfo model = GetIndexModel();

            return Task.FromResult<object>(View["Index.sshtml", model]);
        }

        private Task<object> OnDrivePage(dynamic parameters, CancellationToken token)
        {
            try
            {
                _log.WriteLineDebug("Drive: {0}", parameters.drive);

                DriveInfo model = GetDrive(parameters.drive);

                _log.WriteLineDebug("Drive: {0} exists={1} canmount={2}", model.Drive, model.Exists, model.CanMount);

                return Task.FromResult<object>(View["Drive.sshtml", model]);
            }
            catch (Exception ex)
            {
                _log.WriteLineError(ex, "Drive: {0}", parameters.drive);
                throw;
            }
        }

        private Task<object> OnDriveStatusGet(dynamic parameters, CancellationToken token)
        {
            return GetDriveStatus(parameters.drive);
        }

        private Task<object> GetDriveStatus(string drive)
        {
            _log.WriteLineDebug("DriveStatusGet: {0}", drive);

            DriveInfo model = GetDrive(drive);

            _log.WriteLineDebug("DriveStatusGet: {0} exists={1} canmount={2}", model.Drive, model.Exists, model.CanMount);

            Response response;

            if (model.Exists)
            {
                response = Response.AsJson<object>(new { mounted = true, canMount = false });
            }
            else if (model.CanMount)
            {
                response = Response.AsJson<object>(new { mounted = false, canMount = true });
            }
            else
            {
                response = Response.AsJson<object>(new { mounted = false, canMount = false }, HttpStatusCode.BadRequest);
            }

            return Task.FromResult<object>(response);
        }

        private Task<object> OnDriveStatusPost(dynamic parameters, CancellationToken token)
        {
            _log.WriteLineDebug("DriveStatusPost: {0}", parameters.drive);

            DriveInfo model = GetDrive(parameters.drive);

            _log.WriteLineDebug("DriveStatusPost: {0} exists={1} canmount={2}", model.Drive, model.Exists, model.CanMount);

            Response response;

            if (model.Exists)
            {
                DoUnmount(model.Drive);
                response = HttpStatusCode.OK;
                //response = await GetDriveStatus(model.Drive);
            }
            else if (model.CanMount)
            {
                DoMount(model.Drive);
                response = HttpStatusCode.OK;
                //response = await GetDriveStatus(model.Drive);
            }
            else
            {
                response = Response.AsJson<object>(new { mounted = false, canMount = false }, HttpStatusCode.BadRequest);
            }

            return Task.FromResult<object>(response);
        }

        void DoMount(string letter)
        {
            if (string.IsNullOrEmpty(_config.DriveMountCommand))
                return;

            RunCommand(_config.DriveMountCommand);
        }

        void DoUnmount(string letter)
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

        DriveInfo GetDrive(string driveLetter)
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

        IndexInfo GetIndexModel()
        {
            var roots = new List<RootInfo>();

            var active = _config.ResourceProviders.Where(p => p.Enabled);

            foreach (var resource in active)
            {
                _log.WriteLineDebug("Provider {0}: path: {1}; prefix: {2}; assembly: {3}", resource.Name, resource.RequestRootPath, resource.ResourcePrefix, resource.AssemblyName);

                roots.Add(new RootInfo
                {
                    Title = resource.Name,
                    Url = AdjustRootPath(resource.RequestRootPath)
                });
            }

            var model = new IndexInfo
            {
                Roots = roots
            };
            return model;
        }

        string AdjustRootPath(string path)
        {
            string newpath = path;
            if (path.StartsWith("/"))
                newpath = path.Substring(1);

            if (!newpath.EndsWith("/"))
                newpath = string.Format("{0}/", newpath);

            return newpath;
        }
    }
}
