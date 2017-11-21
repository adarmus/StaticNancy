using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Config
{
    class NancyServiceConfigurationSection : ConfigurationSection
    {
        internal const string CONFIG_SECTION = "nancyService";

        [ConfigurationProperty("port", IsRequired = false)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("drive", IsRequired = false)]
        public string Drive
        {
            get { return (string)this["drive"]; }
            set { this["drive"] = value; }
        }

        [ConfigurationProperty("drivePwd", IsRequired = false)]
        public string DrivePwd
        {
            get { return (string)this["drivePwd"]; }
            set { this["drivePwd"] = value; }
        }

        [ConfigurationProperty("driveMountCommand", IsRequired = false)]
        public string DriveMountCommand
        {
            get { return (string)this["driveMountCommand"]; }
            set { this["driveMountCommand"] = value; }
        }

        [ConfigurationProperty("driveUnmountCommand", IsRequired = false)]
        public string DriveUnmountCommand
        {
            get { return (string)this["driveUnmountCommand"]; }
            set { this["driveUnmountCommand"] = value; }
        }

        [ConfigurationCollection(typeof(ResourceProviderCollection))]
        [ConfigurationProperty("resourceProviders")]
        public ResourceProviderCollection ResourceProviders
        {
            get { return (ResourceProviderCollection)this["resourceProviders"]; }
            set { this["resourceProviders"] = value; }
        }
    }
}
