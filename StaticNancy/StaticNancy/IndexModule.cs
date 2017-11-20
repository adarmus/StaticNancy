using Nancy;
using StaticNancy.Config;
using StaticNancy.Logging;
using StaticNancy.Models;
using System;
using System.Collections.Generic;
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
            this.Get["/Drive/{drive}", runAsync: true] = this.OnDrive;
            this.Get["/Drive/{drive}/status", runAsync: true] = this.OnDriveStatusGet;
            this.Post["/Drive/{drive}/status", runAsync: true] = this.OnDriveStatusPost;
        }

        private Task<object> OnIndex(object parameters, CancellationToken token)
        {
            _log.WriteLineDebug("Index: {0}", parameters);

            IndexInfo model = GetIndexModel();

            return Task.FromResult<object>(View["Index.sshtml", model]);
        }

        private Task<object> OnDriveStatusGet(dynamic parameters, CancellationToken token)
        {
            _log.WriteLineDebug("DriveStatusGet: {0}", parameters.drive);

            DriveInfo model = GetDrive(parameters.drive);

            return Task.FromResult<object>(new { mounted = model.Exists });
        }

        private Task<object> OnDriveStatusPost(dynamic parameters, CancellationToken token)
        {
            _log.WriteLineDebug("DriveStatusPost: {0}", parameters.drive);

            DriveInfo model = GetDrive(parameters.drive);

            if (model.Exists)
            {
                return Task.FromResult<object>(new { mounted = true });
            }
            else
            {
                return Task.FromResult<object>(new { mounted = false });
            }
        }

        private Task<object> OnDrive(dynamic parameters, CancellationToken token)
        {
            _log.WriteLineDebug("Drive: {0}", parameters.drive);

            DriveInfo model = GetDrive(parameters.drive);

            return Task.FromResult<object>(View["Drive.sshtml", model]);
        }

        DriveInfo GetDrive(string driveLetter)
        {
            var drives = System.IO.Directory.GetLogicalDrives();

            var root = System.IO.Directory.GetDirectoryRoot($"{driveLetter}:");

            bool exists = drives.Contains(root);

            return new DriveInfo
            {
                Drive = driveLetter,
                Exists = exists
            };
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
