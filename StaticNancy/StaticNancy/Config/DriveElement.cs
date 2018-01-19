using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Config
{
    class DriveElement : ConfigurationElement
    {
        [ConfigurationProperty("letter", IsRequired = true)]
        public string Letter
        {
            get { return (string)this["letter"]; }
            set { this["letter"] = value; }
        }

        [ConfigurationProperty("drivePwd", IsRequired = true)]
        public string DrivePwd
        {
            get { return (string)this["drivePwd"]; }
            set { this["drivePwd"] = value; }
        }

        [ConfigurationProperty("driveMountCommand", IsRequired = true)]
        public string DriveMountCommand
        {
            get { return (string)this["driveMountCommand"]; }
            set { this["driveMountCommand"] = value; }
        }

        [ConfigurationProperty("driveUnmountCommand", IsRequired = true)]
        public string DriveUnmountCommand
        {
            get { return (string)this["driveUnmountCommand"]; }
            set { this["driveUnmountCommand"] = value; }
        }
    }
}
