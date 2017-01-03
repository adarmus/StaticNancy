using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNancy.Config
{
    class ResourceProviderElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("assemblyName", IsRequired = true)]
        public string AssemblyName
        {
            get { return (string)this["assemblyName"]; }
            set { this["assemblyName"] = value; }
        }

        [ConfigurationProperty("resourcePrefix", IsRequired = true)]
        public string ResourcePrefix
        {
            get { return (string)this["resourcePrefix"]; }
            set { this["resourcePrefix"] = value; }
        }

        [ConfigurationProperty("requestedPath", IsRequired = true)]
        public string RequestedPath
        {
            get { return (string)this["requestedPath"]; }
            set { this["requestedPath"] = value; }
        }

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }
    }
}
